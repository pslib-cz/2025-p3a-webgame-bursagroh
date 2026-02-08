using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Types;
using game.Server.Models;
using game.Server.Requests;
using game.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class DungeonService : IDungeonService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IErrorService _errorService;

    public DungeonService(ApplicationDbContext context, IMapper mapper, IErrorService errorService)
    {
        _context = context;
        _mapper = mapper;
        _errorService = errorService;
    }

    public async Task<ActionResult?> HandleInternalLogic(Player player, Mine? playerMine, MovePlayerRequest request)
    {
        var boundaryError = CheckBoundaries(player, request);
        if (boundaryError != null) return boundaryError;

        if (player.ScreenType == ScreenTypes.Mine)
        {
            bool isExitTile = request.NewPositionX == GameConstants.MineExitX &&
                              request.NewPositionY == GameConstants.MineExitY;

            if (isExitTile)
            {
                var itemsToRemove = player.InventoryItems
                    .Where(ii => ii.ItemInstance.ItemId == 39)
                    .ToList();

                if (itemsToRemove.Any())
                {
                    var instancesToRemove = itemsToRemove.Select(ii => ii.ItemInstance).ToList();

                    _context.InventoryItems.RemoveRange(itemsToRemove);
                    _context.ItemInstances.RemoveRange(instancesToRemove);
                }

                player.ScreenType = ScreenTypes.City;
                player.FloorId = null;
                player.SubPositionX = GameConstants.MineExitX;
                player.SubPositionY = GameConstants.MineExitY;

                await _context.SaveChangesAsync();

                return new OkObjectResult(_mapper.Map<PlayerDto>(player));
            }
        }

        int oldSubX = player.SubPositionX;
        int oldSubY = player.SubPositionY;
        player.SubPositionX = request.NewPositionX;
        player.SubPositionY = request.NewPositionY;

        if (player.ScreenType == ScreenTypes.Floor)
        {
            var buildingExitResult = await ProcessBuildingExit(player);
            if (buildingExitResult != null) return buildingExitResult;
        }

        var context = await GetMovementContextAsync(player, playerMine);

        if (player.ScreenType == ScreenTypes.Mine)
        {
            if (context.BlockedCoordinates.Contains((request.NewPositionX, request.NewPositionY)))
            {
                player.SubPositionX = oldSubX;
                player.SubPositionY = oldSubY;
                return _errorService.CreateErrorResponse(400, 6002, "Movement blocked.", "Path Blocked");
            }
        }

        if (player.FloorId != null)
        {
            await MoveEnemiesOptimizedAsync(player, context);

            var interactionResult = await ProcessFloorInteractions(player, playerMine);
            if (interactionResult != null) return interactionResult;

            await ProcessStairNavigation(player);
        }

        return null; 
    }

    private ActionResult? CheckBoundaries(Player player, MovePlayerRequest request)
    {
        if (player.ScreenType == ScreenTypes.Floor)
        {
            if (request.NewPositionX < 0 || request.NewPositionX > GameConstants.FloorMaxX ||
                request.NewPositionY < 0 || request.NewPositionY > GameConstants.FloorMaxY)
            {
                return _errorService.CreateErrorResponse(400, 6001, "Movement out of bounds.", "Invalid Move");
            }
        }
        else if (player.ScreenType == ScreenTypes.Mine)
        {
            if (request.NewPositionX == 0 && request.NewPositionY == -2)
            {
                return _errorService.CreateErrorResponse(400, 6001, "Movement out of bounds.", "Invalid Move");
            }

            bool isExitTile = request.NewPositionX == GameConstants.MineExitX &&
                              request.NewPositionY == GameConstants.MineExitY;

            if (!isExitTile && (request.NewPositionX < 0 ||
                                request.NewPositionX > GameConstants.MineMaxX ||
                                request.NewPositionY < GameConstants.MineMinY))
            {
                return _errorService.CreateErrorResponse(400, 6001, "Movement out of bounds.", "Invalid Move");
            }
        }

        return null;
    }

    private async Task<ActionResult?> ProcessBuildingExit(Player player)
    {
        var exits = MapGeneratorService.GetExitCoordinates(player.PositionX, player.PositionY);

        if (exits.Any(e => e.x == player.SubPositionX && e.y == player.SubPositionY))
        {
            var floor = await _context.Floors.Include(f => f.Building).FirstOrDefaultAsync(f => f.FloorId == player.FloorId);

            if (floor?.Level == 0)
            {
                if (floor.Building?.BuildingType == BuildingTypes.AbandonedTrap && floor.Building.IsBossDefeated != true)
                {
                    return null;
                }

                player.ScreenType = ScreenTypes.City;
                player.FloorId = null;

                if (player.SubPositionX == 0) player.PositionX -= 1;
                else if (player.SubPositionX == GameConstants.FloorMaxX) player.PositionX += 1;
                else if (player.SubPositionY == 0) player.PositionY -= 1;
                else if (player.SubPositionY == GameConstants.FloorMaxY) player.PositionY += 1;

                player.SubPositionX = 0;
                player.SubPositionY = 0;

                await _context.SaveChangesAsync();
                return new OkObjectResult(_mapper.Map<PlayerDto>(player));
            }
        }
        return null;
    }

    private async Task MoveEnemiesOptimizedAsync(Player player, MovementContext context)
    {
        var exits = MapGeneratorService.GetExitCoordinates(player.PositionX, player.PositionY);
        var stairs = new[] { (GameConstants.StairAX, GameConstants.StairY), (GameConstants.StairBX, GameConstants.StairY) };

        var currentFloor = await _context.Floors
            .AsNoTracking()
            .Include(f => f.Building)
            .FirstOrDefaultAsync(f => f.FloorId == player.FloorId);

        bool canEnemiesStepOnExits = (currentFloor?.Building?.BuildingType == BuildingTypes.AbandonedTrap)
                                     && !(currentFloor?.Building?.IsBossDefeated ?? false);

        var enemies = context.StaticItems.Where(fi => fi.FloorItemType == FloorItemType.Enemy).ToList();
        var occupiedTiles = context.StaticItems
            .Where(fi => fi.FloorItemType != FloorItemType.Enemy && fi.FloorItemType != FloorItemType.Item)
            .Select(fi => (fi.PositionX, fi.PositionY))
            .ToHashSet();

        foreach (var s in stairs) occupiedTiles.Add(s);
        if (!canEnemiesStepOnExits) foreach (var e in exits) occupiedTiles.Add(e);

        var enemyPositions = enemies.Select(e => (e.PositionX, e.PositionY)).ToHashSet();

        foreach (var enemy in enemies)
        {
            int diffX = player.SubPositionX - enemy.PositionX;
            int diffY = player.SubPositionY - enemy.PositionY;

            if (diffX == 0 && diffY == 0) continue;

            var movesToTry = new List<(int x, int y)>();
            if (Math.Abs(diffX) >= Math.Abs(diffY))
            {
                movesToTry.Add((enemy.PositionX + Math.Sign(diffX), enemy.PositionY));
                movesToTry.Add((enemy.PositionX, enemy.PositionY + Math.Sign(diffY)));
            }
            else
            {
                movesToTry.Add((enemy.PositionX, enemy.PositionY + Math.Sign(diffY)));
                movesToTry.Add((enemy.PositionX + Math.Sign(diffX), enemy.PositionY));
            }

            foreach (var move in movesToTry)
            {
                if (move.x < 0 || move.x > GameConstants.FloorMaxX || move.y < 0 || move.y > GameConstants.FloorMaxY) continue;

                bool isPlayer = move.x == player.SubPositionX && move.y == player.SubPositionY;
                bool isBlocked = occupiedTiles.Contains(move) || enemyPositions.Contains(move);

                if (!isBlocked && !isPlayer)
                {
                    enemyPositions.Remove((enemy.PositionX, enemy.PositionY));
                    enemy.PositionX = move.x;
                    enemy.PositionY = move.y;
                    enemyPositions.Add((enemy.PositionX, enemy.PositionY));

                    _context.Entry(enemy).State = EntityState.Modified;
                    break;
                }
            }
        }
    }

    private async Task<MovementContext> GetMovementContextAsync(Player player, Mine? mine)
    {
        var blocked = new HashSet<(int x, int y)>();

        if (player.ScreenType == ScreenTypes.Mine && mine != null)
        {
            var blocks = await _context.MineBlocks
                .Where(mb => mb.MineLayer!.MineId == mine.MineId && mb.MineLayer.Depth == player.SubPositionY)
                .Select(mb => new { mb.Index, mb.MineLayer.Depth })
                .ToListAsync();

            foreach (var b in blocks) blocked.Add((b.Index, (int)b.Depth));
        }

        var items = await _context.FloorItems
            .Where(fi => fi.FloorId == player.FloorId)
            .ToListAsync();

        var enemies = items.Where(i => i.FloorItemType == FloorItemType.Enemy)
                           .Select(i => (i.PositionX, i.PositionY)).ToHashSet();

        return new MovementContext(blocked, enemies, items);
    }

    private async Task<ActionResult?> ProcessFloorInteractions(Player player, Mine? playerMine)
    {
        var floorItem = await _context.FloorItems
            .Include(fi => fi.Chest)
            .Include(fi => fi.Enemy)
            .FirstOrDefaultAsync(fi => fi.FloorId == player.FloorId &&
                                       fi.PositionX == player.SubPositionX &&
                                       fi.PositionY == player.SubPositionY);

        if (floorItem == null) return null;

        if (floorItem.FloorItemType == FloorItemType.Chest && floorItem.Chest != null)
        {
            await HandleChestOpening(player, floorItem);
        }
        else if (floorItem.FloorItemType == FloorItemType.Enemy && floorItem.Enemy != null)
        {
            var random = new Random();
            player.Health -= random.Next(1, 4);

            if (player.Health <= 0)
            {
                player.Health = 0;
                player.ScreenType = ScreenTypes.Lose;
                var itemsToRemove = player.InventoryItems.Where(ii => !ii.IsInBank).ToList();
                if (itemsToRemove.Any()) _context.InventoryItems.RemoveRange(itemsToRemove);
            }
            else player.ScreenType = ScreenTypes.Fight;

            await _context.SaveChangesAsync();
            var fightDto = _mapper.Map<PlayerDto>(player);
            fightDto.MineId = playerMine?.MineId;
            return new OkObjectResult(fightDto);
        }
        return null;
    }

    private async Task HandleChestOpening(Player player, FloorItem floorItem)
    {
        var random = new Random();
        int scatterCount = random.Next(2, 6);
        var floor = await _context.Floors.AsNoTracking().FirstOrDefaultAsync(f => f.FloorId == player.FloorId);
        if (floor == null) return;

        var exits = MapGeneratorService.GetExitCoordinates(player.PositionX, player.PositionY);
        var emptyTiles = new List<(int x, int y)>();

        for (int x = floorItem.PositionX - 1; x <= floorItem.PositionX + 1; x++)
        {
            for (int y = floorItem.PositionY - 1; y <= floorItem.PositionY + 1; y++)
            {
                if (x < 0 || x > GameConstants.FloorMaxX || y < 0 || y > GameConstants.FloorMaxY) continue;
                if (exits.Any(e => e.x == x && e.y == y)) continue;

                bool isOccupied = await _context.FloorItems.AnyAsync(f => f.FloorId == player.FloorId && f.PositionX == x && f.PositionY == y);
                if (!isOccupied) emptyTiles.Add((x, y));
            }
        }

        for (int i = 0; i < scatterCount; i++)
        {
            int randomLootId = GameConstants.ChestLoot[random.Next(GameConstants.ChestLoot.Length)];
            var itemTemplate = await _context.Items.AsNoTracking().FirstOrDefaultAsync(it => it.ItemId == randomLootId);
            if (itemTemplate == null) continue;

            var newItemInstance = new ItemInstance { ItemId = randomLootId, Durability = itemTemplate.MaxDurability };
            _context.ItemInstances.Add(newItemInstance);

            var dropPos = emptyTiles.Any() ? emptyTiles[random.Next(emptyTiles.Count)] : (floorItem.PositionX, floorItem.PositionY);
            if (emptyTiles.Any()) emptyTiles.Remove(dropPos);

            _context.FloorItems.Add(new FloorItem
            {
                FloorId = (int)player.FloorId!,
                PositionX = dropPos.Item1,
                PositionY = dropPos.Item2,
                FloorItemType = FloorItemType.Item,
                ItemInstance = newItemInstance
            });
        }

        _context.Chests.Remove(floorItem.Chest!);
        _context.FloorItems.Remove(floorItem);
    }

    private async Task ProcessStairNavigation(Player player)
    {
        var floor = await _context.Floors.Include(f => f.Building).FirstOrDefaultAsync(f => f.FloorId == player.FloorId);
        if (floor == null) return;

        bool isEven = floor.Level % 2 == 0;
        int upX = isEven ? GameConstants.StairBX : GameConstants.StairAX;
        int downX = isEven ? GameConstants.StairAX : GameConstants.StairBX;

        if (player.SubPositionX == upX && player.SubPositionY == GameConstants.StairY)
        {
            int nextLvl = floor.Level + 1;
            if (floor.Building != null)
            {
                if (!floor.Building.ReachedHeight.HasValue || nextLvl > floor.Building.ReachedHeight.Value)
                    floor.Building.ReachedHeight = nextLvl;
            }

            var nextF = await _context.Floors.FirstOrDefaultAsync(f => f.BuildingId == floor.BuildingId && f.Level == nextLvl);
            if (nextF == null && floor.Building != null && (!floor.Building.Height.HasValue || nextLvl < floor.Building.Height.Value))
            {
                var gen = new MapGeneratorService().GenerateInterior(floor.BuildingId, player.Seed, nextLvl + 1, floor.Building.Height ?? 5, floor.Building.PositionX, floor.Building.PositionY);
                nextF = gen.FirstOrDefault(f => f.Level == nextLvl);
                if (nextF != null)
                {
                    _context.Floors.Add(nextF);
                    await _context.SaveChangesAsync();
                }
            }
            if (nextF != null) player.FloorId = nextF.FloorId;
        }
        else if (player.SubPositionX == downX && player.SubPositionY == GameConstants.StairY && floor.Level > 0)
        {
            var prevF = await _context.Floors.FirstOrDefaultAsync(f => f.BuildingId == floor.BuildingId && f.Level == floor.Level - 1);
            if (prevF != null) player.FloorId = prevF.FloorId;
        }
    }
}