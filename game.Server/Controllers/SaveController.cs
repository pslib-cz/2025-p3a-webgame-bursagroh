using AutoMapper;
using CrypticWizard.RandomWordGenerator;
using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using SQLitePCL;

namespace game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly WordGenerator _generator;
        private readonly IMapper _mapper;

        public SaveController(ApplicationDbContext context, WordGenerator generator, IMapper mapper)
        {
            _context = context;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpPost("/api/Load")]
        public async Task<ActionResult> LoadSnapshot(string saveString, Guid targetPlayerId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var saveEntry = await _context.Saves.AsNoTracking().FirstOrDefaultAsync(s => s.SaveString == saveString);
                if (saveEntry == null) return NotFound("Save string not found.");
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
                return Ok(new { message = "Snapshot loaded using Raw SQL" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        
    }

    [HttpPost]
        public async Task<ActionResult> ClonePlayerRecord(Guid playerId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var originalPlayer = await _context.Players.AsNoTracking()
                    .Include(p => p.Floor)
                    .FirstOrDefaultAsync(p => p.PlayerId == playerId);

                if (originalPlayer == null) return NotFound("Player not found.");

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
                    await LinkPlayerLocationAsync(
                        clonedId,
                        originalPlayer.Floor.Level,
                        originalPlayer.PositionX,
                        originalPlayer.PositionY,
                        originalPlayer.ScreenType
                    );
                }


                var saveStr = string.Join(" ", _generator.GetWords(WordGenerator.PartOfSpeech.noun, 5));
                _context.Saves.Add(new Save { PlayerId = clonedId, SaveString = saveStr });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { SaveString = saveStr, PlayerId = clonedId });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Clone/Load Error: {ex.Message}");
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
                FROM Buildings WHERE PlayerId = {0}", fromId, toId);

            await _context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO Floors (BuildingId, Level)
                SELECT nb.BuildingId, [of].Level
                FROM Floors [of]
                JOIN Buildings ob ON [of].BuildingId = ob.BuildingId
                JOIN Buildings nb ON ob.PositionX = nb.PositionX AND ob.PositionY = nb.PositionY
                WHERE ob.PlayerId = {0} AND nb.PlayerId = {1}", fromId, toId);

            await _context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO FloorItems (FloorId, PositionX, PositionY, FloorItemType)
                SELECT nf.FloorId, fi.PositionX, fi.PositionY, fi.FloorItemType
                FROM FloorItems fi
                JOIN Floors [of] ON fi.FloorId = [of].FloorId
                JOIN Buildings ob ON [of].BuildingId = ob.BuildingId
                JOIN Buildings nb ON ob.PositionX = nb.PositionX AND ob.PositionY = nb.PositionY
                JOIN Floors nf ON nb.BuildingId = nf.BuildingId AND [of].Level = nf.Level
                WHERE ob.PlayerId = {0} AND nb.PlayerId = {1}", fromId, toId);

            await _context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO Enemies (FloorItemId, Health, MaxHealth, EnemyType)
                SELECT nfi.FloorItemId, e.Health, e.MaxHealth, e.EnemyType
                FROM Enemies e
                JOIN FloorItems ofi ON e.FloorItemId = ofi.FloorItemId
                JOIN Floors [of] ON ofi.FloorId = [of].FloorId
                JOIN Buildings ob ON [of].BuildingId = ob.BuildingId
                JOIN Buildings nb ON ob.PositionX = nb.PositionX AND ob.PositionY = nb.PositionY
                JOIN Floors nf ON nb.BuildingId = nf.BuildingId AND [of].Level = nf.Level
                JOIN FloorItems nfi ON nf.FloorId = nfi.FloorId 
                     AND ofi.PositionX = nfi.PositionX 
                     AND ofi.PositionY = nfi.PositionY
                WHERE ob.PlayerId = {0} AND nb.PlayerId = {1}", fromId, toId);

            await _context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO Chests (FloorItemId)
                SELECT nfi.FloorItemId
                FROM Chests c
                JOIN FloorItems ofi ON c.FloorItemId = ofi.FloorItemId
                JOIN Floors [of] ON ofi.FloorId = [of].FloorId
                JOIN Buildings ob ON [of].BuildingId = ob.BuildingId
                JOIN Buildings nb ON ob.PositionX = nb.PositionX AND ob.PositionY = nb.PositionY
                JOIN Floors nf ON nb.BuildingId = nf.BuildingId AND [of].Level = nf.Level
                JOIN FloorItems nfi ON nf.FloorId = nfi.FloorId 
                     AND ofi.PositionX = nfi.PositionX 
                     AND ofi.PositionY = nfi.PositionY
                WHERE ob.PlayerId = {0} AND nb.PlayerId = {1}", fromId, toId);
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

        private async Task TransferMineDataAsync(Guid fromId, Guid toId)
        {
            await _context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO Mines (PlayerId) SELECT {1} FROM Mines WHERE PlayerId = {0}", fromId, toId);

            await _context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO MineLayers (MineId, Depth)
                SELECT nm.MineId, ol.Depth
                FROM MineLayers ol
                JOIN Mines om ON ol.MineId = om.MineId
                JOIN Mines nm ON nm.PlayerId = {1}
                WHERE om.PlayerId = {0}", fromId, toId);

            await _context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO MineBlocks (MineLayerId, BlockId, [Index], Health)
                SELECT nl.MineLayerID, ob.BlockId, ob.[Index], ob.Health
                FROM MineBlocks ob
                JOIN MineLayers ol ON ob.MineLayerId = ol.MineLayerID
                JOIN Mines om ON ol.MineId = om.MineId
                JOIN Mines nm ON nm.PlayerId = {1}
                JOIN MineLayers nl ON nm.MineId = nl.MineId AND ol.Depth = nl.Depth
                WHERE om.PlayerId = {0}", fromId, toId);
        }

        private async Task TransferInventoryAsync(Guid fromId, Guid toId)
        {
            var items = await _context.InventoryItems
                .Where(i => i.PlayerId == fromId)
                .Include(i => i.ItemInstance)
                .AsNoTracking()
                .ToListAsync();

            foreach (var item in items)
            {
                if (item.ItemInstance == null) continue;
                var newInst = new ItemInstance { ItemId = item.ItemInstance.ItemId, Durability = item.ItemInstance.Durability };
                _context.ItemInstances.Add(newInst);
                await _context.SaveChangesAsync();

                _context.InventoryItems.Add(new InventoryItem
                {
                    PlayerId = toId,
                    ItemInstanceId = newInst.ItemInstanceId,
                    IsInBank = item.IsInBank
                });
            }
            await _context.SaveChangesAsync();
        }
    }
}