using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Types;
using game.Server.Models;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class DungeonService : IDungeonService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<DungeonService> _logger;

    public DungeonService(ApplicationDbContext context, IMapper mapper, ILogger<DungeonService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ActionResult?> HandleInternalLogic(Player player, Mine? playerMine, MovePlayerRequest request)
    {
        var mineResult = await ProcessMineLogic(player, playerMine, request);
        if (mineResult != null) return mineResult;

        if (player.FloorId != null)
        {
            player.SubPositionX = request.NewPositionX;
            player.SubPositionY = request.NewPositionY;

            await MoveEnemiesAsync(player);

            var interactionResult = await ProcessFloorInteractions(player, playerMine);
            if (interactionResult != null) return interactionResult;

            var exitResult = await ProcessBuildingExit(player);
            if (exitResult != null) return exitResult;

            await ProcessStairNavigation(player);
        }
        else
        {
            player.SubPositionX = request.NewPositionX;
            player.SubPositionY = request.NewPositionY;
        }

        return null;
    }

    private async Task MoveEnemiesAsync(Player player)
    {
        var enemiesOnFloor = await _context.FloorItems
            .Where(fi => fi.FloorId == player.FloorId && fi.FloorItemType == FloorItemType.Enemy)
            .ToListAsync();

        var obstacles = await _context.FloorItems
            .Where(fi => fi.FloorId == player.FloorId && fi.FloorItemType != FloorItemType.Enemy)
            .Select(fi => new { fi.PositionX, fi.PositionY })
            .ToListAsync();

        var stairs = new[] { (2, 2), (5, 2) };

        foreach (var enemy in enemiesOnFloor)
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
                if (move.x == enemy.PositionX && move.y == enemy.PositionY) continue;
                if (move.x < 0 || move.x > 7 || move.y < 0 || move.y > 7) continue;

                bool isStairs = stairs.Any(s => s.Item1 == move.x && s.Item2 == move.y);
                bool isObstacle = obstacles.Any(o => o.PositionX == move.x && o.PositionY == move.y);
                bool isOtherEnemy = enemiesOnFloor.Any(e => e.FloorItemId != enemy.FloorItemId && e.PositionX == move.x && e.PositionY == move.y);

                if (!isStairs && !isObstacle && !isOtherEnemy)
                {
                    enemy.PositionX = move.x;
                    enemy.PositionY = move.y;
                    _context.Entry(enemy).State = EntityState.Modified;
                    break;
                }
            }
        }
    }

    private async Task<ActionResult?> ProcessFloorInteractions(Player player, Mine? playerMine)
    {
        var floorItem = _context.FloorItems.Local
            .FirstOrDefault(fi => fi.FloorId == player.FloorId &&
                                 fi.PositionX == player.SubPositionX &&
                                 fi.PositionY == player.SubPositionY)
            ?? await _context.FloorItems
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
            _logger.LogWarning("Combat collision detected at ({X}, {Y})", player.SubPositionX, player.SubPositionY);

            var random = new Random();
            player.Health -= random.Next(1, 4);

            if (player.Health <= 0)
            {
                player.Health = 0;
                player.ScreenType = ScreenTypes.Lose;
                var itemsToRemove = player.InventoryItems.Where(ii => !ii.IsInBank).ToList();
                if (itemsToRemove.Any()) _context.InventoryItems.RemoveRange(itemsToRemove);
            }
            else
            {
                player.ScreenType = ScreenTypes.Fight;
            }

            await _context.SaveChangesAsync();
            var fightDto = _mapper.Map<PlayerDto>(player);
            fightDto.MineId = playerMine?.MineId;
            return new OkObjectResult(fightDto);
        }
        return null;
    }

    private async Task<ActionResult?> ProcessMineLogic(Player player, Mine? playerMine, MovePlayerRequest request)
    {
        if (player.ScreenType == ScreenTypes.Mine && request.NewPositionX == 0 && request.NewPositionY == 0)
        {
            player.ScreenType = ScreenTypes.City;
            player.FloorId = null;
            player.SubPositionX = 0;
            player.SubPositionY = 0;
            if (playerMine != null) _context.Mines.Remove(playerMine);
            await _context.SaveChangesAsync();
            return new OkObjectResult(_mapper.Map<PlayerDto>(player));
        }

        if (player.ScreenType == ScreenTypes.Mine && playerMine != null)
        {
            var blockAtTarget = await _context.MineBlocks
                .AnyAsync(mb => mb.MineLayer.MineId == playerMine.MineId &&
                                mb.MineLayer.Depth == request.NewPositionY &&
                                mb.Index == request.NewPositionX);
            if (blockAtTarget) return new BadRequestObjectResult("Movement blocked by a mine block.");
        }
        return null;
    }

    private async Task HandleChestOpening(Player player, FloorItem floorItem)
    {
        var random = new Random();
        int[] lootIds = { 10, 20, 30, 11, 21, 31, 12, 22, 32, 40, 41, 42 };
        int scatterCount = random.Next(2, 6);

        var floor = await _context.Floors.AsNoTracking()
            .FirstOrDefaultAsync(f => f.FloorId == player.FloorId);
        if (floor == null) return;

        bool isEven = floor.Level % 2 == 0;
        var stairsUp = (x: isEven ? 5 : 2, y: 2);
        var stairsDown = (x: isEven ? 2 : 5, y: 2);
        var exits = MapGeneratorService.GetExitCoordinates(player.PositionX, player.PositionY);

        var emptyTiles = new List<(int x, int y)>();
        for (int x = floorItem.PositionX - 1; x <= floorItem.PositionX + 1; x++)
        {
            for (int y = floorItem.PositionY - 1; y <= floorItem.PositionY + 1; y++)
            {
                if (x < 0 || x > 7 || y < 0 || y > 7) continue;
                if (exits.Any(e => e.x == x && e.y == y)) continue;
                if ((x == stairsUp.x && y == stairsUp.y) || (x == stairsDown.x && y == stairsDown.y)) continue;

                bool isOccupied = await _context.FloorItems.AnyAsync(f =>
                    f.FloorId == player.FloorId && f.PositionX == x && f.PositionY == y);

                if (!isOccupied) emptyTiles.Add((x, y));
            }
        }

        for (int i = 0; i < scatterCount; i++)
        {
            int randomLootId = lootIds[random.Next(lootIds.Length)];
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

    private async Task<ActionResult?> ProcessBuildingExit(Player player)
    {
        var exits = MapGeneratorService.GetExitCoordinates(player.PositionX, player.PositionY);
        if (player.ScreenType == ScreenTypes.Floor && exits.Any(e => e.x == player.SubPositionX && e.y == player.SubPositionY))
        {
            var floor = await _context.Floors.Include(f => f.Building).FirstOrDefaultAsync(f => f.FloorId == player.FloorId);
            if (floor?.Level == 0)
            {
                if (floor.Building?.BuildingType == BuildingTypes.AbandonedTrap) return new BadRequestObjectResult("The door is locked!");

                player.ScreenType = ScreenTypes.City;
                player.FloorId = null;

                if (player.SubPositionX == 0) player.PositionX -= 1;
                else if (player.SubPositionX == 7) player.PositionX += 1;
                else if (player.SubPositionY == 0) player.PositionY -= 1;
                else if (player.SubPositionY == 7) player.PositionY += 1;

                player.SubPositionX = 0;
                player.SubPositionY = 0;

                await _context.SaveChangesAsync();
                return new OkObjectResult(_mapper.Map<PlayerDto>(player));
            }
        }
        return null;
    }

    private async Task ProcessStairNavigation(Player player)
    {
        var floor = await _context.Floors.FirstOrDefaultAsync(f => f.FloorId == player.FloorId);
        if (floor == null) return;

        bool isEven = floor.Level % 2 == 0;
        if (player.SubPositionX == (isEven ? 5 : 2) && player.SubPositionY == 2)
        {
            int nextLvl = floor.Level + 1;
            var nextF = await _context.Floors.FirstOrDefaultAsync(f => f.BuildingId == floor.BuildingId && f.Level == nextLvl);

            if (nextF == null)
            {
                var b = await _context.Buildings.FindAsync(floor.BuildingId);
                if (b != null && (!b.Height.HasValue || nextLvl < b.Height.Value))
                {
                    var gen = new MapGeneratorService().GenerateInterior(b.BuildingId, player.Seed, nextLvl + 1, b.Height ?? 5, b.PositionX, b.PositionY);
                    nextF = gen.FirstOrDefault(f => f.Level == nextLvl);
                    if (nextF != null) { _context.Floors.Add(nextF); await _context.SaveChangesAsync(); }
                }
            }
            if (nextF != null) player.FloorId = nextF.FloorId;
        }
        else if (player.SubPositionX == (isEven ? 2 : 5) && player.SubPositionY == 2 && floor.Level > 0)
        {
            var prevF = await _context.Floors.FirstOrDefaultAsync(f => f.BuildingId == floor.BuildingId && f.Level == floor.Level - 1);
            if (prevF != null) player.FloorId = prevF.FloorId;
        }
    }
}