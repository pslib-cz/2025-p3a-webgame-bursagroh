using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;

namespace game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static readonly SemaphoreSlim _bulkSemaphore = new SemaphoreSlim(1, 1);

        public BuildingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<IEnumerable<Building>>> GetPlayerBuildings(Guid playerId, [FromQuery] int top, [FromQuery] int left, [FromQuery] int width, [FromQuery] int height)
        {
            var playerExists = await _context.Players.AsNoTracking().AnyAsync(p => p.PlayerId == playerId);
            if (!playerExists) return NotFound();

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
            var proceduralBuildings = mapGenerator.GenerateMapArea(playerId, minX, maxX, minY, maxY);

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

            return Ok(existingBuildings);
        }

        [HttpGet("{buildingId}/Interior/{level}")]
        public async Task<ActionResult<Floor>> GetSpecificFloor(Guid playerId, int buildingId, int level)
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

            if (level > 0)
            {
                var currentPlayerFloor = await _context.Floors.FindAsync(player.FloorId);

                if ((currentPlayerFloor == null || currentPlayerFloor.BuildingId != buildingId || currentPlayerFloor.Level != level - 1) && currentPlayerFloor.Level != level)
                {
                    return BadRequest($"Cannot generate floor {level} unless you are standing on floor {level - 1}. Do not forget that you have to use the player move endpoint to get assigned into the 0th level.");
                }
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
                .FirstOrDefaultAsync(f => f.BuildingId == buildingId && f.Level == level);

            if (targetFloor != null)
            {
                return Ok(targetFloor);
            }

            int totalHeight = building.Height ?? 5;

            var mapGenerator = new MapGeneratorService();
            int combinedSeed = buildingId + level;

            var generatedFloors = mapGenerator.GenerateInterior(buildingId, combinedSeed, level + 1, totalHeight);
            var newFloor = generatedFloors.FirstOrDefault(f => f.Level == level);

            if (newFloor != null)
            {
                _context.Floors.Add(newFloor);
                building.ReachedHeight = level;

                await _context.SaveChangesAsync();
                return Ok(newFloor);
            }

            return StatusCode(500, "Error generating floor.");
        }

        [HttpPatch("{id}/Action/interact")]
        public async Task<ActionResult> Interact(Guid id, [FromBody] InteractionRequest request)
        {
            var player = await _context.Players
                .Include(p => p.InventoryItems)
                    .ThenInclude(ii => ii.ItemInstance)
                        .ThenInclude(ins => ins.Item)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) return NotFound("Player not found");

            var floorItem = await _context.FloorItems
                .Include(fi => fi.Enemy)
                .FirstOrDefaultAsync(fi => fi.FloorId == player.FloorId &&
                                           fi.PositionX == request.TargetX &&
                                           fi.PositionY == request.TargetY &&
                                           fi.FloorItemType == FloorItemType.Enemy);

            if (floorItem?.Enemy == null) {
                return BadRequest("No enemy at target coordinates.");
            }
            

            var targetEnemy = floorItem.Enemy;

            if (targetEnemy.EnemyType != EnemyType.Zombie &&
                targetEnemy.EnemyType != EnemyType.Skeleton &&
                targetEnemy.EnemyType != EnemyType.Dragon)
            {
                return BadRequest("Invalid enemy type.");
            }

            int diffX = Math.Abs(player.SubPositionX - request.TargetX);
            int diffY = Math.Abs(player.SubPositionY - request.TargetY);

            bool isWithinReach = (diffX + diffY) <= 1;

            if (!isWithinReach) {
                return BadRequest("Enemy is too far away.");
            } 


            int damageDealt = 1;

            if (request.InventoryItemId.HasValue && request.InventoryItemId.Value != 0)
            {
                var chosenItem = player.InventoryItems
                    .FirstOrDefault(ii => ii.InventoryItemId == request.InventoryItemId.Value);

                if (chosenItem != null && chosenItem.ItemInstance?.Item != null &&
                    chosenItem.ItemInstance.Item.Name.Contains("Sword"))
                {
                    damageDealt = chosenItem.ItemInstance.Item.Damage;
                }
                else
                {
                    return BadRequest("Selected item is not a sword. Use null for fists.");
                }
            }

            targetEnemy.Health -= damageDealt;

            if (targetEnemy.Health > 0)
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = $"You hit the {targetEnemy.EnemyType} for {damageDealt} damage.",
                    remainingHealth = targetEnemy.Health
                });
            }

            var lootInstance = await _context.ItemInstances
                .Include(i => i.Item)
                .FirstOrDefaultAsync(i => i.ItemInstanceId == targetEnemy.ItemInstanceId);

            if (lootInstance != null)
            {
                floorItem.FloorItemType = FloorItemType.Item;
                floorItem.Enemy = null;
                floorItem.ItemInstanceId = lootInstance.ItemInstanceId;
                _context.Enemies.Remove(targetEnemy);
            }
            else
            {
                _context.FloorItems.Remove(floorItem);
                _context.Enemies.Remove(targetEnemy);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Successfully defeated the {targetEnemy.EnemyType}!",
                lootDropped = lootInstance?.Item?.Name ?? "Nothing"
            });
        }
    }
}
