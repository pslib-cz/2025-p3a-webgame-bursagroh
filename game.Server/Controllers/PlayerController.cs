using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Models;
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

        public PlayerController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private List<(int x, int y)> GetExitCoordinates(int buildingX, int buildingY)
        {
            var exits = new List<(int x, int y)>();

            if (IsRoad(buildingX, buildingY - 1)) exits.Add((3, 0)); 
            if (IsRoad(buildingX, buildingY - 1)) exits.Add((4, 0));

            if (IsRoad(buildingX, buildingY + 1)) exits.Add((3, 7));
            if (IsRoad(buildingX, buildingY + 1)) exits.Add((4, 7));

            if (IsRoad(buildingX - 1, buildingY)) exits.Add((0, 3));
            if (IsRoad(buildingX - 1, buildingY)) exits.Add((0, 4));

            if (IsRoad(buildingX + 1, buildingY)) exits.Add((7, 3)); 
            if (IsRoad(buildingX + 1, buildingY)) exits.Add((7, 4));

            if (!exits.Any()) exits.Add((0, 0));

            return exits;
        }

        private bool IsRoad(int x, int y)
        {
            return Math.Abs(x) % 4 == 1 || Math.Abs(y) % 4 == 1;
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

                return Ok(_mapper.Map<PlayerDto>(player));
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
            } catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
            
        }

        /// <remarks>
        /// - zmena obrazovky
        /// - pri vstupu do city resetuje i subposX a subposY na 0
        /// - pokud je hrac v AbandonedTrap tak nemuze odejit z budovy
        /// </remarks>
        [HttpPatch("{id}/Action/move-screen")]
        public async Task<ActionResult> MoveScreen(Guid id, [FromBody] MoveScreenRequest request)
        {
            try
            {
                var player = await _context.Players
                .Include(p => p.Floor)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

                if (player == null) return NotFound();

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

                if (request.NewScreenType == ScreenTypes.City)
                {
                    if (player.ScreenType == ScreenTypes.Floor)
                    {
                        var currentFloor = await _context.Floors.FindAsync(player.FloorId);
                        if (currentFloor == null || currentFloor.Level != 0)
                            return BadRequest("You can only leave the building from the ground floor (Level 0).");

                        if (player.SubPositionX != 0 || player.SubPositionY != 0)
                            return BadRequest("You must be at the entrance (0, 0) to leave the building.");

                        var building = await _context.Buildings.FirstOrDefaultAsync(b =>
                            b.PositionX == player.PositionX && b.PositionY == player.PositionY && b.PlayerId == id);

                        if (building != null && building.BuildingType == BuildingTypes.AbandonedTrap)
                            return BadRequest("This building is a trap!");
                    }

                    player.FloorId = null;
                    player.SubPositionX = 0;
                    player.SubPositionY = 0;
                }

                if (request.NewScreenType == ScreenTypes.Floor && player.ScreenType == ScreenTypes.City)
                {
                    var building = await _context.Buildings.FirstOrDefaultAsync(b => b.PositionX == player.PositionX && b.PositionY == player.PositionY && b.PlayerId == id);

                    if (building == null) return BadRequest("No building here.");

                    var floor0 = await _context.Floors
                        .FirstOrDefaultAsync(f => f.BuildingId == building.BuildingId && f.Level == 0);

                    if (floor0 == null)
                    {
                        var mapGenerator = new MapGeneratorService();
                        int totalHeight = building.Height ?? 5;

                        var generatedFloors = mapGenerator.GenerateInterior(building.BuildingId, building.BuildingId, 1, totalHeight, building.PositionX, building.PositionY);
                        floor0 = generatedFloors.FirstOrDefault(f => f.Level == 0);

                        if (floor0 == null) return StatusCode(500, "Generation failed.");

                        _context.Floors.Add(floor0);
                        building.ReachedHeight = 0;

                        await _context.SaveChangesAsync();
                    }

                    var buildingDoors = this.GetExitCoordinates(building.PositionX, building.PositionY);
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
            } catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
            
        }

        /// <remarks>
        /// - pohyb 
        /// - pokud je floorId na null, tak se meni PositionX a PositionY 
        /// - jinak se meni patro a pozice jsou ignorovany 
        /// </remarks>
        [HttpPatch("{id}/Action/move")]
        public async Task<ActionResult<PlayerDto>> MovePlayer(Guid id, [FromBody] MovePlayerRequest request)
        {
            try
            {
                var player = await _context.Players
                .Include(p => p.Floor)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

                if (player == null) return NotFound();

                if (player.ScreenType == ScreenTypes.Floor && request.NewFloorId.HasValue && request.NewFloorId != player.FloorId)
                {
                    var currentFloor = await _context.Floors.FirstOrDefaultAsync(f => f.FloorId == player.FloorId);
                    var targetFloor = await _context.Floors.FirstOrDefaultAsync(f => f.FloorId == request.NewFloorId);

                    if (targetFloor == null || currentFloor == null) return BadRequest("Floor doesn't exist.");
                    if (targetFloor.BuildingId != currentFloor.BuildingId) return BadRequest("Cannot move between buildings vertically.");

                    int levelDiff = targetFloor.Level - currentFloor.Level;
                    if (levelDiff == 1 && (player.SubPositionX != 5 || player.SubPositionY != 2)) return BadRequest("Must be at (5,2) to go UP.");
                    if (levelDiff == -1 && (player.SubPositionX != 2 || player.SubPositionY != 2)) return BadRequest("Must be at (2,2) to go DOWN.");
                    if (Math.Abs(levelDiff) > 1) return BadRequest("One floor at a time.");

                    player.FloorId = request.NewFloorId;
                    await _context.SaveChangesAsync();
                    return Ok(_mapper.Map<PlayerDto>(player));
                }

                int currentX = (player.ScreenType == ScreenTypes.City) ? player.PositionX : player.SubPositionX;
                int currentY = (player.ScreenType == ScreenTypes.City) ? player.PositionY : player.SubPositionY;

                if ((Math.Abs(request.NewPositionX - currentX) + Math.Abs(request.NewPositionY - currentY)) != 1)
                    return BadRequest("Move must be exactly 1 square.");

                if (player.ScreenType == ScreenTypes.City)
                {
                    player.PositionX = request.NewPositionX;
                    player.PositionY = request.NewPositionY;

                    if (player.PositionX == 0 && player.PositionY == 0)
                    {
                        player.ScreenType = ScreenTypes.Fountain;
                    }

                    var building = await _context.Buildings.FirstOrDefaultAsync(b =>
                        b.PositionX == player.PositionX && b.PositionY == player.PositionY && b.PlayerId == id);

                    if (building != null)
                    {
                        switch (building.BuildingType)
                        {
                            case BuildingTypes.Abandoned:
                            case BuildingTypes.AbandonedTrap:
                                var floor0 = await _context.Floors.FirstOrDefaultAsync(f => f.BuildingId == building.BuildingId && f.Level == 0);
                                if (floor0 == null)
                                {
                                    var mapGen = new MapGeneratorService();
                                    var generated = mapGen.GenerateInterior(building.BuildingId, building.BuildingId, 1, building.Height ?? 5, building.PositionX, building.PositionY);
                                    floor0 = generated.First(f => f.Level == 0);

                                    _context.Floors.Add(floor0);
                                    await _context.SaveChangesAsync();
                                }

                                var buildingDoors = MapGeneratorService.GetExitCoordinates(building.PositionX, building.PositionY);
                                var spawnPoint = buildingDoors.First();

                                player.FloorId = floor0.FloorId;
                                player.ScreenType = ScreenTypes.Floor;
                                player.SubPositionX = spawnPoint.x;
                                player.SubPositionY = spawnPoint.y;
                                break;

                            case BuildingTypes.Bank: player.ScreenType = ScreenTypes.Bank; break;
                            case BuildingTypes.Mine: player.ScreenType = ScreenTypes.Mine; break;
                            case BuildingTypes.Restaurant: player.ScreenType = ScreenTypes.Restaurant; break;
                            case BuildingTypes.Blacksmith: player.ScreenType = ScreenTypes.Blacksmith; break;
                        }
                    }
                }
                else
                {
                    var exits = MapGeneratorService.GetExitCoordinates(player.PositionX, player.PositionY);
                    bool isAtExit = exits.Any(e => e.x == request.NewPositionX && e.y == request.NewPositionY);

                    if (player.ScreenType == ScreenTypes.Floor && isAtExit)
                    {
                        var currentFloor = await _context.Floors.FindAsync(player.FloorId);
                        if (currentFloor?.Level == 0)
                        {
                            player.ScreenType = ScreenTypes.City;
                            player.FloorId = null;
                            player.SubPositionX = 0;
                            player.SubPositionY = 0;
                            await _context.SaveChangesAsync();
                            return Ok(_mapper.Map<PlayerDto>(player));
                        }
                    }

                    player.SubPositionX = request.NewPositionX;
                    player.SubPositionY = request.NewPositionY;
                }

                await _context.SaveChangesAsync();

                var dto = _mapper.Map<PlayerDto>(player);
                var mine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id);
                dto.MineId = mine?.MineId;

                return Ok(dto);
            } catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
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
            } catch (Exception ex)
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
            } catch (Exception ex)
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
                if (player == null)
                {
                    return NotFound("Player not found.");
                }
                if (player.FloorId == null)
                {
                    return BadRequest("You can only drop items while in floor/mine.");
                }



                var inventoryItem = await _context.InventoryItems
                    .Include(ii => ii.ItemInstance)
                    .FirstOrDefaultAsync(ii => ii.InventoryItemId == request.InventoryItemId && ii.PlayerId == id);

                if (inventoryItem == null)
                {
                    return BadRequest("Item not found in the inventory.");
                }

                if (player.ActiveInventoryItemId == inventoryItem.InventoryItemId)
                {
                    player.ActiveInventoryItemId = null;
                }

                var floorItem = new FloorItem
                {
                    FloorId = player.FloorId.Value,
                    PositionX = player.SubPositionX,
                    PositionY = player.SubPositionY,
                    FloorItemType = FloorItemType.Item,
                    ItemInstanceId = inventoryItem.ItemInstanceId
                };

                _context.InventoryItems.Remove(inventoryItem);
                _context.FloorItems.Add(floorItem);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Item dropped.",
                    itemInstanceId = inventoryItem.ItemInstanceId,
                    x = player.SubPositionX,
                    y = player.SubPositionY
                });
            } catch (Exception ex)
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
            } catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpPatch("{id}/Action/use")]
        public async Task<ActionResult<PlayerDto>> UseActiveItem(Guid id)
        {
            try
            {
                var player = await _context.Players
                    .Include(p => p.ActiveInventoryItem)
                        .ThenInclude(ai => ai.ItemInstance)
                    .FirstOrDefaultAsync(p => p.PlayerId == id);

                if (player == null) return NotFound("Player not found.");

                if (player.ActiveInventoryItemId == null || player.ActiveInventoryItem == null)
                {
                    return BadRequest("You aren't holding anything to use.");
                }

                var activeItem = player.ActiveInventoryItem;

                if (activeItem.ItemInstance.ItemId == 40)
                {
                    if (player.Health >= 20)
                    {
                        return BadRequest("You are already at full health!");
                    }

                    player.Health = Math.Min(20, player.Health + 5);
                    var instanceToDelete = activeItem.ItemInstance;
                    player.ActiveInventoryItemId = null;

                    _context.InventoryItems.Remove(activeItem);
                    _context.ItemInstances.Remove(instanceToDelete);
                }
                else
                {
                    return BadRequest($"The {activeItem.ItemInstance.ItemId} is not a consumable item.");
                }

                await _context.SaveChangesAsync();

                var dto = _mapper.Map<PlayerDto>(player);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}