
using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Models;
using game.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace game.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        private readonly IDungeonService _dungeonService;

        public PlayerController(ApplicationDbContext context, IMapper mapper, ICityService cityService, IDungeonService dungeonService)
        {
            _context = context;
            _mapper = mapper;
            _cityService = cityService;
            _dungeonService = dungeonService;

        }


        [HttpPost("Generate")]
        public async Task<ActionResult<PlayerDto>> Generate([FromBody] GeneratePlayerRequest request)
        {
            try
            {
                Player player = new Player
                {
                    PlayerId = Guid.NewGuid(),
                    Name = request.Name,
                    ScreenType = ScreenTypes.City,
                    Money = 0,
                    BankBalance = 0,
                    Capacity = 10,
                    Seed = new Random().Next(),
                    Health = 10,

                    PositionX = 0,
                    PositionY = 0,
                    SubPositionX = 0,
                    SubPositionY = 0,
                };

                var fixedBuildings = new List<Building>
            {
                new Building { PlayerId = player.PlayerId, BuildingType = BuildingTypes.Fountain, PositionX = 0, PositionY = 0, IsBossDefeated = false },
                new Building { PlayerId = player.PlayerId, BuildingType = BuildingTypes.Mine, PositionX = 2, PositionY = 0, IsBossDefeated = false },
                new Building { PlayerId = player.PlayerId, BuildingType = BuildingTypes.Bank, PositionX = -2, PositionY = 0, IsBossDefeated = false },
                new Building { PlayerId = player.PlayerId, BuildingType = BuildingTypes.Restaurant, PositionX = 0, PositionY = -2, IsBossDefeated = false },
                new Building { PlayerId = player.PlayerId, BuildingType = BuildingTypes.Blacksmith, PositionX = 0, PositionY = 2, IsBossDefeated = false }
            };

                _context.Players.Add(player);
                _context.Buildings.AddRange(fixedBuildings);


                await _context.SaveChangesAsync();

                Mine mine = new Mine
                {
                    MineId = new Random().Next(),
                    PlayerId = player.PlayerId
                };
                _context.Mines.Add(mine);

                var playerDto2 = _mapper.Map<PlayerDto>(player);
                playerDto2.MineId = mine.MineId;

                return Ok(playerDto2);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(Guid id)
        {
            try
            {
                var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);
                if (player == null) return NotFound();

                var dto = _mapper.Map<PlayerDto>(player);

                var mine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id);
                dto.MineId = mine?.MineId;

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpPatch("{id}/Action/move-screen")]
        public async Task<ActionResult> MoveScreen(Guid id, [FromBody] MoveScreenRequest request)
        {
            try
            {
                var player = await _context.Players
                    .Include(p => p.Floor)
                    .FirstOrDefaultAsync(p => p.PlayerId == id);

                if (player == null) return NotFound();

                if (player.ScreenType == ScreenTypes.Lose && request.NewScreenType == ScreenTypes.City)
                {
                    player.ScreenType = ScreenTypes.City;
                    player.PositionX = 0;
                    player.PositionY = 0;
                    player.FloorId = null;
                }

                if (player.ScreenType == ScreenTypes.Mine && request.NewScreenType != ScreenTypes.Mine)
                {
                    var rentedItems = await _context.InventoryItems
                        .Include(ii => ii.ItemInstance)
                        .Where(ii => ii.PlayerId == id && ii.ItemInstance.ItemId == 39)
                        .ToListAsync();

                    if (rentedItems.Any())
                    {
                        var instances = rentedItems.Select(ri => ri.ItemInstance).ToList();
                        _context.InventoryItems.RemoveRange(rentedItems);
                        _context.ItemInstances.RemoveRange(instances);
                        await _context.SaveChangesAsync();
                    }
                }

                if (request.NewScreenType == ScreenTypes.City && player.ScreenType == ScreenTypes.Floor)
                {
                    var currentFloor = await _context.Floors.FindAsync(player.FloorId);
                    if (currentFloor == null || currentFloor.Level != 0)
                        return BadRequest("You can only leave the building from the ground floor (Level 0).");

                    var validExits = MapGeneratorService.GetExitCoordinates(player.PositionX, player.PositionY);
                    bool isAtExit = validExits.Any(e => e.x == player.SubPositionX && e.y == player.SubPositionY);

                    if (!isAtExit)
                        return BadRequest("You must be at an entrance to leave the building.");

                    var building = await _context.Buildings.FirstOrDefaultAsync(b =>
                        b.PositionX == player.PositionX && b.PositionY == player.PositionY && b.PlayerId == id);

                    if (building != null && building.BuildingType == BuildingTypes.AbandonedTrap)
                        return BadRequest("This building is a trap!");

                    
                    if (player.SubPositionX == 0) player.PositionX--;
                    else if (player.SubPositionX == 7) player.PositionX++;
                    else if (player.SubPositionY == 0) player.PositionY--;
                    else if (player.SubPositionY == 7) player.PositionY++;

                    player.FloorId = null;
                    player.SubPositionX = 0;
                    player.SubPositionY = 0;
                }

                if (request.NewScreenType == ScreenTypes.Floor && player.ScreenType == ScreenTypes.City)
                {
                    var building = await _context.Buildings.FirstOrDefaultAsync(b => b.PositionX == player.PositionX && b.PositionY == player.PositionY && b.PlayerId == id);
                    if (building == null) return BadRequest("No building here.");

                    var floor0 = await _context.Floors.FirstOrDefaultAsync(f => f.BuildingId == building.BuildingId && f.Level == 0);

                    if (floor0 == null)
                    {
                        var mapGenerator = new MapGeneratorService();
                        var generatedFloors = mapGenerator.GenerateInterior(building.BuildingId, building.BuildingId, 1, building.Height ?? 5, building.PositionX, building.PositionY);
                        floor0 = generatedFloors.FirstOrDefault(f => f.Level == 0);
                        if (floor0 == null) return StatusCode(500, "Generation failed.");

                        _context.Floors.Add(floor0);
                        building.ReachedHeight = 0;
                        await _context.SaveChangesAsync();
                    }

                    var buildingDoors = MapGeneratorService.GetExitCoordinates(building.PositionX, building.PositionY);
                    var spawnPoint = buildingDoors.First();

                    player.FloorId = floor0.FloorId;
                    player.SubPositionX = spawnPoint.x;
                    player.SubPositionY = spawnPoint.y;
                }

                player.ScreenType = request.NewScreenType;
                await _context.SaveChangesAsync();

                var dto = _mapper.Map<PlayerDto>(player);
                var mine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id);
                dto.MineId = mine?.MineId;

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{id}/Action/move")]
        public async Task<ActionResult<PlayerDto>> MovePlayer(Guid id, [FromBody] MovePlayerRequest request)
        {
            try
            {
                var player = await _context.Players
                    .Include(p => p.Floor)
                    .Include(p => p.InventoryItems)
                    .FirstOrDefaultAsync(p => p.PlayerId == id);

                if (player == null) return NotFound();

                var playerMine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id);

                int currentX = (player.ScreenType == ScreenTypes.City) ? player.PositionX : player.SubPositionX;
                int currentY = (player.ScreenType == ScreenTypes.City) ? player.PositionY : player.SubPositionY;

                if ((Math.Abs(request.NewPositionX - currentX) + Math.Abs(request.NewPositionY - currentY)) != 1)
                    return BadRequest("Move must be exactly 1 square.");

                if (player.ScreenType == ScreenTypes.City)
                {
                    await _cityService.HandleCityMovement(player, request, id);
                }
                else
                {
                    var result = await _dungeonService.HandleInternalLogic(player, playerMine, request);
                    if (result != null) return result;
                }

                await _context.SaveChangesAsync();

                var dto = _mapper.Map<PlayerDto>(player);
                dto.MineId = (await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id))?.MineId;
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}/Inventory")]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetPlayerInventory(Guid id)
        {
            try
            {
                var items = await _context.InventoryItems
                .Where(i => i.PlayerId == id && !i.IsInBank)
                .Include(i => i.ItemInstance)
                    .ThenInclude(ins => ins!.Item)
                .ToListAsync();

                if (items == null || !items.Any())
                {
                    return NoContent();
                }

                var inventoryDtos = _mapper.Map<List<InventoryItemDto>>(items);

                return Ok(inventoryDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        public class PickRequest //proc to tu je?
        {
            public int FloorItemId { get; set; }

        }

        [HttpPatch("{id}/Action/pick")]
        public async Task<ActionResult> Pick(Guid id, [FromBody] PickRequest request)
        {
            try
            {
                var player = await _context.Players
            .Include(p => p.InventoryItems)
            .FirstOrDefaultAsync(p => p.PlayerId == id);

                if (player == null)
                {
                    return NotFound("Player not found in database.");
                }

                if (player.FloorId == null)
                {
                    return BadRequest("Player is not on a floor.");
                }



                var itemsOnGround = await _context.FloorItems
                    .Where(fi => fi.FloorId == player.FloorId &&
                                 fi.PositionX == player.SubPositionX &&
                                 fi.PositionY == player.SubPositionY &&
                                 fi.FloorItemId == request.FloorItemId &&
                                 fi.FloorItemType == FloorItemType.Item)
                    .ToListAsync();

                if (!itemsOnGround.Any()) return BadRequest("Nothing here to pick up.");

                int currentInventoryCount = player.InventoryItems.Count;

                foreach (var floorItem in itemsOnGround)
                {

                    if (currentInventoryCount >= player.Capacity) break;

                    var instanceId = floorItem.ItemInstanceId;
                    if (instanceId == null) continue;


                    _context.InventoryItems.Add(new InventoryItem
                    {
                        PlayerId = id,
                        ItemInstanceId = instanceId.Value,
                        IsInBank = false
                    });

                    _context.FloorItems.Remove(floorItem);
                    currentInventoryCount++;
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest($"Database Error: {ex.InnerException?.Message ?? ex.Message}");
                }

                return Ok(new { message = "Items picked up.", newCount = currentInventoryCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpPatch("{id}/Action/drop")]
        public async Task<ActionResult> Drop(Guid id, [FromBody] DropItemRequest request)
        {
            try
            {
                var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);
                if (player == null) return NotFound("Player not found.");

                var inventoryItem = await _context.InventoryItems
                    .Include(ii => ii.ItemInstance)
                    .FirstOrDefaultAsync(ii => ii.InventoryItemId == request.InventoryItemId && ii.PlayerId == id);

                if (inventoryItem == null) return BadRequest("Item not found in the inventory.");

                bool isWinCondition = inventoryItem.ItemInstance.ItemId == 100 &&
                                      player.SubPositionX == 0 &&
                                      player.SubPositionY == 0;

                if (isWinCondition)
                {
                    player.ScreenType = ScreenTypes.Win;
                }
                else if (player.FloorId == null)
                {
                    return BadRequest("You can only drop items while in floor/mine.");
                }

                if (player.ActiveInventoryItemId == inventoryItem.InventoryItemId)
                {
                    player.ActiveInventoryItemId = null;
                }

                if (player.FloorId != null)
                {
                    var floorItem = new FloorItem
                    {
                        FloorId = player.FloorId.Value,
                        PositionX = player.SubPositionX,
                        PositionY = player.SubPositionY,
                        FloorItemType = FloorItemType.Item,
                        ItemInstanceId = inventoryItem.ItemInstanceId
                    };
                    _context.FloorItems.Add(floorItem);
                }

                _context.InventoryItems.Remove(inventoryItem);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = player.ScreenType == ScreenTypes.Win ? "You won the game!" : "Item dropped.",
                    currentScreen = player.ScreenType.ToString(),
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("{id}/Action/set-active-item")]
        public async Task<ActionResult<PlayerDto>> SetActiveItem(Guid id, [FromBody] SetActiveItemRequest request)
        {
            try
            {
                var player = await _context.Players
                .FirstOrDefaultAsync(p => p.PlayerId == id);

                if (player == null) return NotFound("Player not found.");

                if (request.InventoryItemId == null)
                {
                    player.ActiveInventoryItemId = null;
                }
                else
                {
                    var itemExists = await _context.InventoryItems
                        .AnyAsync(ii => ii.InventoryItemId == request.InventoryItemId && ii.PlayerId == id);

                    if (!itemExists)
                    {
                        return BadRequest("Item not found in your inventory.");
                    }

                    player.ActiveInventoryItemId = request.InventoryItemId;
                }

                await _context.SaveChangesAsync();

                var dto = _mapper.Map<PlayerDto>(player);
                var mine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id);
                dto.MineId = mine?.MineId;

                return Ok(dto);
            }
            catch (Exception ex)
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

                if (player.ActiveInventoryItem?.ItemInstance?.Item == null)
                {
                    return BadRequest("You aren't holding anything to use.");
                }

                var activeItem = player.ActiveInventoryItem;
                var itemData = activeItem.ItemInstance.Item;

                if (itemData.ItemType == ItemTypes.Sword)
                {
                    if (player.ScreenType != ScreenTypes.Fight)
                    {
                        return BadRequest("You can only use weapons during a fight.");
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
                    int damageDealt = itemData.Damage;
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

                else if (itemData.ItemId == 40 || itemData.ItemId == 41 || itemData.ItemId == 42)
                {
                    switch (itemData.ItemId)
                    {
                        case 40:
                            if (player.Health >= player.MaxHealth) return BadRequest("Already at full health!");
                            player.Health = Math.Min(player.MaxHealth, player.Health + 5);
                            break;

                        case 41:
                            player.MaxHealth += 5;
                            break;

                        case 42:
                            player.Capacity += 5;
                            break;
                    }

                    var instanceToDelete = activeItem.ItemInstance;
                    player.ActiveInventoryItemId = null;
                    _context.InventoryItems.Remove(activeItem);
                    _context.ItemInstances.Remove(instanceToDelete);

                    await _context.SaveChangesAsync();

                    var dto = _mapper.Map<PlayerDto>(player);
                    return Ok(dto);
                }

                return BadRequest($"The {itemData.Name} is not a usable or consumable item.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}