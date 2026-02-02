using AutoMapper;
using EFCore.BulkExtensions;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static readonly SemaphoreSlim _bulkSemaphore = new SemaphoreSlim(1, 1);
        private readonly IMapper _mapper;

        public BuildingController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildingDto>>> GetPlayerBuildings(Guid playerId, [FromQuery] int top, [FromQuery] int left, [FromQuery] int width, [FromQuery] int height)
        {
            try
            {
                var player = await _context.Players.FindAsync(playerId);
                if (player == null) return NotFound();

                int minX = left;
                int maxX = left + width - 1;
                int minY = top;
                int maxY = top + height - 1;

                var existingBuildings = await _context.Buildings
                    .AsNoTracking()
                    .Where(b => b.PlayerId == playerId &&
                                b.PositionX >= minX && b.PositionX <= maxX &&
                                b.PositionY >= minY && b.PositionY <= maxY)
                    .ToListAsync();

                var mapGenerator = new MapGeneratorService();
                var proceduralBuildings = mapGenerator.GenerateMapArea(playerId, minX, maxX, minY, maxY, player.Seed);

                var existingCoords = new HashSet<(int, int)>(
                    existingBuildings.Select(b => (b.PositionX, b.PositionY))
                );

                var newBuildings = proceduralBuildings
                    .Where(pb => !existingCoords.Contains((pb.PositionX, pb.PositionY)))
                    .ToList();

                if (newBuildings.Any())
                {
                    await _bulkSemaphore.WaitAsync();
                    try
                    {
                        await _context.BulkInsertAsync(newBuildings, config =>
                        {
                            config.SetOutputIdentity = true;
                        });
                    }
                    finally
                    {
                        _bulkSemaphore.Release();
                    }

                    existingBuildings.AddRange(newBuildings);
                }

                var buildingDtos = _mapper.Map<IEnumerable<BuildingDto>>(existingBuildings);
                return Ok(buildingDtos);
            } catch
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{buildingId}/Interior/{level}")]
        public async Task<ActionResult<FloorDto>> GetSpecificFloor(Guid playerId, int buildingId, int level)
        {
            try
            {
                var player = await _context.Players.FindAsync(playerId);

                if (player == null)
                {
                    return NotFound("Player not found.");
                }

                var building = await _context.Buildings.FindAsync(buildingId);

                if (building == null || building.PlayerId != playerId)
                {
                    return NotFound("Building not found for this player.");
                }

                if (player.PositionX != building.PositionX || player.PositionY != building.PositionY)
                {
                    return BadRequest("Player is not at the building's location.");
                }

                if (player.ScreenType != ScreenTypes.Floor)
                {
                    return BadRequest("Player must be in Floor screen to access interior floors.");
                }

                if ((building.BuildingType != BuildingTypes.Abandoned) && (building.BuildingType != BuildingTypes.AbandonedTrap))
                {
                    return BadRequest("Interior floors can only be accessed for Abandoned or AbandonedTrap building types.");
                }

                if (building.Height.HasValue && level >= building.Height.Value)
                {
                    return BadRequest($"Building only has {building.Height} floors (Levels 0 to {building.Height - 1}). Level {level} is out of bounds.");
                }

                var floorsBelowCount = await _context.Floors
                    .Where(f => f.BuildingId == buildingId && f.Level < level)
                    .CountAsync();

                if (floorsBelowCount < level)
                {
                    return BadRequest($"Cannot access floor {level} because floors below it have not been cleared or generated yet.");
                }

                var targetFloor = await _context.Floors
                    .Include(f => f.FloorItems!)
                        .ThenInclude(fi => fi.Chest)
                    .Include(f => f.FloorItems!)
                        .ThenInclude(fi => fi.Enemy)
                            .ThenInclude(e => e!.ItemInstance)
                                .ThenInclude(ii => ii!.Item)
                    .Include(f => f.FloorItems!)
                        .ThenInclude(fi => fi.ItemInstance)
                            .ThenInclude(ii => ii!.Item)
                    .FirstOrDefaultAsync(f => f.BuildingId == buildingId && f.Level == level);

                if (targetFloor != null)
                {
                    return Ok(_mapper.Map<FloorDto>(targetFloor));
                }

                int totalHeight = building.Height ?? 5;

                var mapGenerator = new MapGeneratorService();
                int combinedSeed = buildingId + level;

                var generatedFloors = mapGenerator.GenerateInterior(buildingId, combinedSeed, level + 1, totalHeight, building.PositionX, building.PositionY);
                var newFloor = generatedFloors.FirstOrDefault(f => f.Level == level);

                if (newFloor != null)
                {
                    _context.Floors.Add(newFloor);
                    building.ReachedHeight = level;
                    await _context.SaveChangesAsync();


                    return Ok(_mapper.Map<FloorDto>(newFloor));
                }

                return StatusCode(500, "Failed to generate the requested floor.");
            } catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("Floor/{floorId}")]
        public async Task<ActionResult<FloorDto>> GetFloorById(int floorId)
        {
            try
            {
                var floor = await _context.Floors
                .AsNoTracking()
                .Include(f => f.FloorItems!)
                    .ThenInclude(fi => fi.Chest)
                .Include(f => f.FloorItems!)
                    .ThenInclude(fi => fi.Enemy)
                        .ThenInclude(e => e!.ItemInstance)
                            .ThenInclude(ii => ii!.Item)
                .Include(f => f.FloorItems!)
                    .ThenInclude(fi => fi.ItemInstance)
                        .ThenInclude(ii => ii!.Item)
                .FirstOrDefaultAsync(f => f.FloorId == floorId);

                if (floor == null)
                {
                    return NotFound($"Floor with ID {floorId} not found.");
                }

                return Ok(_mapper.Map<FloorDto>(floor));
            } catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpPatch("{id}/Action/interact")]
        public async Task<ActionResult> Interact(Guid id, [FromBody] InteractionRequest request)
        {
            try {
                var player = await _context.Players
                .Include(p => p.InventoryItems)
                    .ThenInclude(ii => ii.ItemInstance)
                        .ThenInclude(ins => ins.Item)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

                if (player == null) return NotFound("Player not found");

                var floorItem = await _context.FloorItems
                    .Include(fi => fi.Enemy)
                    .Include(fi => fi.Chest)
                    .FirstOrDefaultAsync(fi => fi.FloorId == player.FloorId &&
                                               fi.PositionX == request.TargetX &&
                                               fi.PositionY == request.TargetY);

                if (floorItem == null) return BadRequest("Nothing to interact with here.");

                if (floorItem.FloorItemType == FloorItemType.Chest && floorItem.Chest != null)
                {
                    if (player.SubPositionX != request.TargetX || player.SubPositionY != request.TargetY)
                    {
                        return BadRequest("You must be standing directly on the chest to open it.");
                    }

                    var random = new Random();

                    int[] ids = { 10, 11, 12, 13, 14, 15, 16 };

                    int scatterCount = random.Next(2, 6);

                    var emptyTiles = new List<(int x, int y)>();
                    for (int x = floorItem.PositionX - 1; x <= floorItem.PositionX + 1; x++)
                    {
                        for (int y = floorItem.PositionY - 1; y <= floorItem.PositionY + 1; y++)
                        {
                            bool isOccupied = await _context.FloorItems.AnyAsync(f =>
                                f.FloorId == player.FloorId && f.PositionX == x && f.PositionY == y);

                            if (!isOccupied) emptyTiles.Add((x, y));
                        }
                    }

                    for (int i = 0; i < scatterCount; i++)
                    {
                        int randomSwordId = ids[random.Next(ids.Length)];

                        var itemTemplate = await _context.Items.AsNoTracking()
                            .FirstOrDefaultAsync(it => it.ItemId == randomSwordId);

                        if (itemTemplate == null) continue;

                        var newItemInstance = new ItemInstance
                        {
                            ItemId = randomSwordId,
                            Durability = itemTemplate.MaxDurability,
                            ChestId = null
                        };

                        _context.ItemInstances.Add(newItemInstance);

                        (int x, int y) dropPos = (floorItem.PositionX, floorItem.PositionY);
                        if (emptyTiles.Any())
                        {
                            int index = random.Next(emptyTiles.Count);
                            dropPos = emptyTiles[index];
                            emptyTiles.RemoveAt(index);
                        }

                        _context.FloorItems.Add(new FloorItem
                        {
                            FloorId = (int)player.FloorId!,
                            PositionX = dropPos.x,
                            PositionY = dropPos.y,
                            FloorItemType = FloorItemType.Item,
                            ItemInstance = newItemInstance
                        });
                    }

                    _context.Chests.Remove(floorItem.Chest);
                    _context.FloorItems.Remove(floorItem);

                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Chest exploded!", itemsDropped = scatterCount });
                }

                if (floorItem.FloorItemType == FloorItemType.Enemy && floorItem.Enemy != null)
                {
                    if (player.SubPositionX == request.TargetX && player.SubPositionY == request.TargetY)
                    {
                        player.ScreenType = ScreenTypes.Fight;
                        await _context.SaveChangesAsync();

                        return Ok(new
                        {
                            message = "Entering combat!",
                            screenType = ScreenTypes.Fight.ToString(),
                            enemyType = floorItem.Enemy.EnemyType.ToString()
                        });
                    }
                    else
                    {
                        return BadRequest("You must step on the enemy to initiate a fight.");
                    }
                }

                return BadRequest("Invalid interaction.");

            } catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{id}/Action/use")]
        public async Task<ActionResult> UseItem(Guid id)
        {
            try
            {
                var player = await _context.Players
                    .Include(p => p.ActiveInventoryItem)
                        .ThenInclude(ai => ai.ItemInstance)
                            .ThenInclude(ii => ii.Item)
                    .FirstOrDefaultAsync(p => p.PlayerId == id);

                if (player == null) return NotFound("Player not found.");

                if (player.ScreenType != ScreenTypes.Fight)
                {
                    return BadRequest("You can only use items during a fight.");
                }

                var floorItem = await _context.FloorItems
                    .Include(fi => fi.Enemy)
                    .FirstOrDefaultAsync(fi => fi.FloorId == player.FloorId &&
                                               fi.PositionX == player.SubPositionX &&
                                               fi.PositionY == player.SubPositionY);

                if (floorItem?.Enemy == null)
                {
                    player.ScreenType = ScreenTypes.Floor;
                    await _context.SaveChangesAsync();
                    return BadRequest("No enemy found here.");
                }

                var targetEnemy = floorItem.Enemy;
                int damageDealt = 1;
                if (player.ActiveInventoryItem?.ItemInstance?.Item != null)
                {
                    if (player.ActiveInventoryItem.ItemInstance.Item.ItemType == ItemTypes.Sword)
                    {
                        damageDealt = player.ActiveInventoryItem.ItemInstance.Item.Damage;
                    }
                }

                targetEnemy.Health -= damageDealt;

                if (targetEnemy.Health > 0)
                {
                    await _context.SaveChangesAsync();
                    return Ok(new
                    {
                        message = $"You hit the {targetEnemy.EnemyType} for {damageDealt} damage!",
                        enemyHealth = targetEnemy.Health
                    });
                }

                if (targetEnemy.ItemInstanceId.HasValue)
                {
                    floorItem.FloorItemType = FloorItemType.Item;
                    floorItem.ItemInstanceId = targetEnemy.ItemInstanceId;
                    floorItem.Enemy = null;
                }
                else
                {
                    _context.FloorItems.Remove(floorItem);
                }

                _context.Enemies.Remove(targetEnemy);
                player.ScreenType = ScreenTypes.Floor;

                await _context.SaveChangesAsync();

                return Ok(new { message = $"Defeated the {targetEnemy.EnemyType}!", victory = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
