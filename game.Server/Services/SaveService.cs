using CrypticWizard.RandomWordGenerator;
using game.Server.Data;
using game.Server.Models;
using game.Server.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8620 
#pragma warning disable CS8602 
public class SaveService : ISaveService
{
    private readonly ApplicationDbContext _context;
    private readonly WordGenerator _generator;

    public SaveService(ApplicationDbContext context, WordGenerator generator)
    {
        _context = context;
        _generator = generator;
    }

    public async Task<ActionResult> LoadSnapshotAsync(string saveString, Guid targetPlayerId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var saveEntry = await _context.Saves.AsNoTracking().FirstOrDefaultAsync(s => s.SaveString == saveString);
            if (saveEntry == null) return new NotFoundObjectResult("Save string not found.");

            var sourceId = saveEntry.PlayerId;

            await SyncPlayerStatsAsync(sourceId, targetPlayerId);
            await TransferWorldDataAsync(sourceId, targetPlayerId);
            await TransferMineDataAsync(sourceId, targetPlayerId);
            await TransferInventoryAsync(sourceId, targetPlayerId);

            var sourceInfo = await _context.Players.AsNoTracking()
                .Select(p => new { p.PlayerId, p.PositionX, p.PositionY, p.ScreenType, Level = p.Floor != null ? p.Floor.Level : 0 })
                .FirstOrDefaultAsync(p => p.PlayerId == sourceId);

            if (sourceInfo != null)
            {
                await LinkPlayerLocationAsync(targetPlayerId, sourceInfo.Level, sourceInfo.PositionX, sourceInfo.PositionY, sourceInfo.ScreenType);
            }

            await transaction.CommitAsync();
            return new OkObjectResult(new { message = "Snapshot loaded." });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ObjectResult($"Internal server error: {ex.Message}") { StatusCode = 500 };
        }
    }

    public async Task<ActionResult> ClonePlayerRecordAsync(Guid playerId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var originalPlayer = await _context.Players.AsNoTracking()
                .Include(p => p.Floor)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (originalPlayer == null) return new NotFoundObjectResult("Player not found.");

            var clonedId = Guid.NewGuid();
            var clonedPlayer = new Player();

            _context.Entry(clonedPlayer).CurrentValues.SetValues(originalPlayer);
            clonedPlayer.PlayerId = clonedId;
            clonedPlayer.Name = originalPlayer.Name;
            clonedPlayer.FloorId = null;

            _context.Players.Add(clonedPlayer);
            await _context.SaveChangesAsync();

            await TransferWorldDataAsync(playerId, clonedId);
            await TransferMineDataAsync(playerId, clonedId);
            await TransferInventoryAsync(playerId, clonedId);

            if (originalPlayer.FloorId.HasValue && originalPlayer.Floor != null)
            {
                await LinkPlayerLocationAsync(clonedId, originalPlayer.Floor.Level, originalPlayer.PositionX, originalPlayer.PositionY, originalPlayer.ScreenType);
            }

            var saveStr = string.Join(" ", _generator.GetWords(WordGenerator.PartOfSpeech.noun, 5));
            _context.Saves.Add(new Save { PlayerId = clonedId, SaveString = saveStr });

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new OkObjectResult(new { SaveString = saveStr, PlayerId = clonedId });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ObjectResult($"Clone/Load Error: {ex.Message}") { StatusCode = 500 };
        }
    }

    private async Task LinkPlayerLocationAsync(Guid playerId, int level, int x, int y, ScreenTypes screenType)
    {
        if (screenType == ScreenTypes.Mine)
        {
            await _context.Database.ExecuteSqlRawAsync(@"
                UPDATE Players SET FloorId = (
                    SELECT f.FloorId FROM Floors f
                    JOIN Buildings b ON f.BuildingId = b.BuildingId
                    WHERE b.PlayerId = {0} AND b.BuildingType = 4
                    LIMIT 1
                ) WHERE PlayerId = {0}", playerId);
        }
        else
        {
            await _context.Database.ExecuteSqlRawAsync(@"
                UPDATE Players SET FloorId = (
                    SELECT f.FloorId FROM Floors f
                    JOIN Buildings b ON f.BuildingId = b.BuildingId
                    WHERE b.PlayerId = {0} AND b.PositionX = {1} AND b.PositionY = {2} AND f.Level = {3}
                    LIMIT 1
                ) WHERE PlayerId = {0}", playerId, x, y, level);
        }
    }

    private async Task TransferWorldDataAsync(Guid fromId, Guid toId)
    {
        await _context.Database.ExecuteSqlRawAsync(@"
        INSERT INTO Buildings (BuildingType, Height, IsBossDefeated, PlayerId, PositionX, PositionY, ReachedHeight)
        SELECT BuildingType, Height, IsBossDefeated, {1}, PositionX, PositionY, ReachedHeight
        FROM Buildings ob WHERE ob.PlayerId = {0}
        AND NOT EXISTS (SELECT 1 FROM Buildings nb WHERE nb.PlayerId = {1} AND nb.PositionX = ob.PositionX AND nb.PositionY = ob.PositionY)", fromId, toId);

        await _context.Database.ExecuteSqlRawAsync(@"
        INSERT INTO Floors (BuildingId, Level)
        SELECT nb.BuildingId, [of].Level
        FROM Floors [of]
        JOIN Buildings ob ON [of].BuildingId = ob.BuildingId
        JOIN Buildings nb ON ob.PositionX = nb.PositionX AND ob.PositionY = nb.PositionY AND nb.PlayerId = {1}
        WHERE ob.PlayerId = {0}
        AND NOT EXISTS (SELECT 1 FROM Floors nf WHERE nf.BuildingId = nb.BuildingId AND nf.Level = [of].Level)", fromId, toId);

        var sourceFloors = await _context.Floors
            .Include(f => f.Building)
            .Include(f => f.FloorItems).ThenInclude(fi => fi.Enemy)
            .Include(f => f.FloorItems).ThenInclude(fi => fi.Chest)
            .Include(f => f.FloorItems).ThenInclude(fi => fi.ItemInstance)
            .Where(f => f.Building.PlayerId == fromId)
            .AsNoTracking()
            .ToListAsync();

        var targetFloors = await _context.Floors
            .Include(f => f.Building)
            .Where(f => f.Building.PlayerId == toId)
            .ToListAsync();

        foreach (var oldFloor in sourceFloors)
        {
            var newFloor = targetFloors.FirstOrDefault(f =>
                f.Level == oldFloor.Level &&
                f.Building.PositionX == oldFloor.Building.PositionX &&
                f.Building.PositionY == oldFloor.Building.PositionY);

            if (newFloor == null) continue;

            foreach (var oldFi in oldFloor.FloorItems)
            {
                if (await _context.FloorItems.AnyAsync(fi => fi.FloorId == newFloor.FloorId && fi.PositionX == oldFi.PositionX && fi.PositionY == oldFi.PositionY))
                    continue;

                var newFi = new FloorItem
                {
                    FloorId = newFloor.FloorId,
                    PositionX = oldFi.PositionX,
                    PositionY = oldFi.PositionY,
                    FloorItemType = oldFi.FloorItemType
                };

                if (oldFi.Enemy != null)
                {
                    newFi.Enemy = new Enemy { Health = oldFi.Enemy.Health, MaxHealth = oldFi.Enemy.MaxHealth, EnemyType = oldFi.Enemy.EnemyType };
                }
                if (oldFi.Chest != null)
                {
                    newFi.Chest = new Chest();
                }

                if (oldFi.ItemInstance != null)
                {
                    newFi.ItemInstance = new ItemInstance { ItemId = oldFi.ItemInstance.ItemId, Durability = oldFi.ItemInstance.Durability };
                }

                _context.FloorItems.Add(newFi);
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task TransferMineDataAsync(Guid fromId, Guid toId)
    {
        var hasMine = await _context.Mines.AnyAsync(m => m.PlayerId == toId);
        if (!hasMine)
        {
            _context.Mines.Add(new Mine { PlayerId = toId });
            await _context.SaveChangesAsync();
        }

        var sourceMine = await _context.Mines
            .Include(m => m.MineLayers).ThenInclude(ml => ml.MineBlocks)
            .FirstOrDefaultAsync(m => m.PlayerId == fromId);

        var targetMine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == toId);

        if (sourceMine == null || targetMine == null) return;

        foreach (var oldLayer in sourceMine.MineLayers)
        {
            var newLayer = await _context.MineLayers
                .FirstOrDefaultAsync(l => l.MineId == targetMine.MineId && l.Depth == oldLayer.Depth);

            if (newLayer == null)
            {
                newLayer = new MineLayer { MineId = targetMine.MineId, Depth = oldLayer.Depth };
                _context.MineLayers.Add(newLayer);
                await _context.SaveChangesAsync();
            }

            foreach (var oldBlock in oldLayer.MineBlocks)
            {
                if (!await _context.MineBlocks.AnyAsync(mb => mb.MineLayerId == newLayer.MineLayerID && mb.Index == oldBlock.Index))
                {
                    _context.MineBlocks.Add(new MineBlock
                    {
                        MineLayerId = newLayer.MineLayerID,
                        BlockId = oldBlock.BlockId,
                        Index = oldBlock.Index,
                        Health = oldBlock.Health
                    });
                }
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SyncPlayerStatsAsync(Guid sourcePlayerId, Guid targetPlayerId)
    {
        await _context.Database.ExecuteSqlRawAsync(@"
            UPDATE Players 
            SET Money = s.Money, 
                BankBalance = s.BankBalance, 
                ScreenType = s.ScreenType,
                PositionX = s.PositionX, 
                PositionY = s.PositionY, 
                SubPositionX = s.SubPositionX,
                SubPositionY = s.SubPositionY, 
                Capacity = s.Capacity, 
                Seed = s.Seed,
                Health = s.Health, 
                MaxHealth = s.MaxHealth, 
                Name = s.Name,
                FloorId = NULL, 
                ActiveInventoryItemId = NULL
            FROM (SELECT * FROM Players WHERE PlayerId = {1}) AS s
            WHERE Players.PlayerId = {0}", targetPlayerId, sourcePlayerId);
    }


    private async Task TransferInventoryAsync(Guid fromId, Guid toId)
    {
        var sourceItems = await _context.InventoryItems
            .Where(i => i.PlayerId == fromId)
            .Include(i => i.ItemInstance)
            .AsNoTracking()
            .ToListAsync();

        var targetItems = await _context.InventoryItems
            .Where(i => i.PlayerId == toId)
            .Include(i => i.ItemInstance)
            .ToListAsync();

        foreach (var sItem in sourceItems)
        {
            if (sItem.ItemInstance == null) continue;

            bool alreadyOwned = targetItems.Any(t =>
                t.ItemInstance.ItemId == sItem.ItemInstance.ItemId &&
                t.IsInBank == sItem.IsInBank);

            if (!alreadyOwned)
            {
                var newInst = new ItemInstance
                {
                    ItemId = sItem.ItemInstance.ItemId,
                    Durability = sItem.ItemInstance.Durability
                };
                _context.ItemInstances.Add(newInst);
                await _context.SaveChangesAsync();

                _context.InventoryItems.Add(new InventoryItem
                {
                    PlayerId = toId,
                    ItemInstanceId = newInst.ItemInstanceId,
                    IsInBank = sItem.IsInBank
                });
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task<ActionResult> DeletePlayerDataAsync(Guid playerId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == playerId);
            if (player == null) return new NotFoundObjectResult("Player not found.");

            player.ActiveInventoryItemId = null;
            player.FloorId = null;
            await _context.SaveChangesAsync();

            var inventoryItems = await _context.InventoryItems
                .Where(ii => ii.PlayerId == playerId)
                .ToListAsync();

            var inventoryInstanceIds = inventoryItems.Select(ii => ii.ItemInstanceId).ToList();

            var playerFloorIds = await _context.Floors
                .Where(f => f.Building.PlayerId == playerId)
                .Select(f => f.FloorId)
                .ToListAsync();

            var floorItemInstances = await _context.FloorItems
                .Where(fi => playerFloorIds.Contains(fi.FloorId) && fi.ItemInstanceId != null)
                .Select(fi => fi.ItemInstanceId!.Value)
                .ToListAsync();

            _context.InventoryItems.RemoveRange(inventoryItems);

            var allInstanceIds = inventoryInstanceIds.Concat(floorItemInstances).Distinct();
            var instancesToDelete = await _context.ItemInstances
                .Where(ii => allInstanceIds.Contains(ii.ItemInstanceId))
                .ToListAsync();
            _context.ItemInstances.RemoveRange(instancesToDelete);

            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Saves WHERE PlayerId = {0}", playerId);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Mines WHERE PlayerId = {0}", playerId);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Buildings WHERE PlayerId = {0}", playerId);

            _context.Players.Remove(player);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new OkObjectResult(new { message = "Purge complete." });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ObjectResult($"Purge Failed: {ex.Message}") { StatusCode = 500 };
        }
    }
}