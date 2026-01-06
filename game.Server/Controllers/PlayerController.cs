using game.Server.Data;
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
        private readonly ApplicationDbContext context;

        public PlayerController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost("Generate")]
        public async Task<Player> Generate([FromBody] GeneratePlayerRequest request)
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

            context.Players.Add(player);
            context.Buildings.AddRange(fixedBuildings);
            await context.SaveChangesAsync();



            context.Players.Update(player);
            await context.SaveChangesAsync();

            return player;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(Guid id) {
            Player? player = await context.Players.Where(p => p.PlayerId == id).FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpGet("ScreenTypes")]
        public ActionResult<IEnumerable<string>> GetScreenTypes()
        {
            string[] screenTypes = Enum.GetNames<ScreenTypes>();
            return Ok(screenTypes);
        }

        /// <remarks>
        /// - zmena obrazovky
        /// - pri vstupu do city resetuje i subposX a subposY na 0
        /// - pokud je hrac v AbandonedTrap tak nemuze odejit z budovy
        /// </remarks>
        [HttpPatch("{id}/Action/move-screen")]
        public async Task<ActionResult> MoveScreen(Guid id, [FromBody] MoveScreenRequest newScreenType)
        {
            Player? player = await context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            if (player.ScreenType == ScreenTypes.Floor && newScreenType.NewScreenType == ScreenTypes.City)
            {
                var currentFloor = await context.Floors.FindAsync(player.FloorId);

                if (currentFloor == null || currentFloor.Level != 0)
                {
                    return BadRequest("You can only leave the building from the ground floor (Level 0).");
                }

                if (player.SubPositionX != 0 || player.SubPositionY != 0)
                {
                    return BadRequest("You must be at the entrance (0, 0) to leave the building.");
                }

                var building = await context.Buildings.FirstOrDefaultAsync(b =>
                    b.PositionX == player.PositionX &&
                    b.PositionY == player.PositionY &&
                    b.PlayerId == id);

                if (building != null && building.BuildingType == BuildingTypes.AbandonedTrap)
                {
                    return BadRequest("This building is a trap!");
                }

                player.FloorId = null;
            }

            if (player.ScreenType == ScreenTypes.Mine)
            {
                player.FloorId = null;
            }

            player.ScreenType = newScreenType.NewScreenType;

            if (newScreenType.NewScreenType == ScreenTypes.City)
            {
                player.SubPositionX = 0;
                player.SubPositionY = 0;
            }

            await context.SaveChangesAsync();

            return Ok(player);
        }

        /// <remarks>
        /// - pohyb 
        /// - pokud je floorId na null, tak se meni PositionX a PositionY 
        /// - jinak se meni patro a pozice jsou ignorovany 
        /// </remarks>
        [HttpPatch("{id}/Action/move")]
        public async Task<ActionResult<Player>> MovePlayer(Guid id, [FromBody] MovePlayerRequest request)
        {
            Player? player = await context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);
            if (player == null) return NotFound();

            if (player.ScreenType == ScreenTypes.Floor && request.NewFloorId.HasValue && request.NewFloorId != player.FloorId)
            {
                var currentFloor = await context.Floors.FirstOrDefaultAsync(f => f.FloorId == player.FloorId);
                var targetFloor = await context.Floors.FirstOrDefaultAsync(f => f.FloorId == request.NewFloorId);

                if (player.FloorId != null)
                {
                    if (targetFloor == null || currentFloor == null)
                    {
                        return BadRequest("Current or target floor doesn't exist.");
                    }

                    if (targetFloor.BuildingId != currentFloor.BuildingId)
                    {
                        return BadRequest("You can't move between buildings like this.");
                    }

                    int levelDifference = targetFloor.Level - currentFloor.Level;

                    if (levelDifference == 1)
                    {
                        if (player.SubPositionX != 5 || player.SubPositionY != 2)
                        {
                            return BadRequest("You must be at position (5, 2) to move UP a floor.");
                        }
                    }
                    else if (levelDifference == -1)
                    {
                        if (player.SubPositionX != 2 || player.SubPositionY != 2)
                        {
                            return BadRequest("You must be at position (2, 2) to move DOWN a floor.");
                        }
                    }
                    else
                    {
                        return BadRequest("You can move only one floor at a time.");
                    }
                }

                player.FloorId = request.NewFloorId;
                await context.SaveChangesAsync();
                return Ok(player); 
            }

            if (player.ScreenType == ScreenTypes.Floor && (request.NewPositionX < 0 || request.NewPositionX > 7 || request.NewPositionY < 0 || request.NewPositionY > 7))
            {
                return BadRequest("Coordinates are out of bounds.");
            }

            int currentX = (player.ScreenType == ScreenTypes.City) ? player.PositionX : player.SubPositionX;
            int currentY = (player.ScreenType == ScreenTypes.City) ? player.PositionY : player.SubPositionY;

            bool isAdjacent = (Math.Abs(request.NewPositionX - currentX) +
                               Math.Abs(request.NewPositionY - currentY)) == 1;

            if (!isAdjacent)
            {
                return BadRequest("Move must be exactly 1 square.");
            }

            if (player.ScreenType == ScreenTypes.Mine)
            {
                if (request.NewPositionX < 0 || request.NewPositionX > 7)
                {
                    return BadRequest("Coordinates are out of bounds.");
                }

                var mineBlockAtDestination = await context.Mines
                    .Where(m => m.PlayerId == id)
                    .SelectMany(m => m.MineLayers)
                    .Where(l => l.Depth == request.NewPositionY)
                    .SelectMany(l => l.MineBlocks)
                    .AnyAsync(mb => mb.Index == request.NewPositionX);

                if (mineBlockAtDestination)
                {
                    return BadRequest("You must mine this block before moving onto it.");
                }
            }

            if (player.ScreenType == ScreenTypes.City)
            {
                player.PositionX = request.NewPositionX;
                player.PositionY = request.NewPositionY;
            }
            else
            {
                player.SubPositionX = request.NewPositionX;
                player.SubPositionY = request.NewPositionY;
            }

            await context.SaveChangesAsync();
            return Ok(player);
        }

        [HttpGet("{id}/Inventory")]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetPlayerInventory(Guid id)
        {
            var items = await context.InventoryItems
                .Where(i => i.PlayerId == id && !i.IsInBank)
                .Include(i => i.ItemInstance)        
                    .ThenInclude(ins => ins.Item)
                .ToListAsync();

            if (items == null || !items.Any())
            {
                return NoContent(); 
            }

            return Ok(items);
        }

        [HttpPatch("{id}/Action/pick")]
        public async Task<ActionResult> Pick(Guid id) 
        {

            var player = await context.Players
                .Include(p => p.InventoryItems)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) {
                return NotFound("Player not found in database.");
            }

            if (player.FloorId == null) {
                return BadRequest("Player is not on a floor.");
            }
            

            
            var itemsOnGround = await context.FloorItems
                .Where(fi => fi.FloorId == player.FloorId &&
                             fi.PositionX == player.SubPositionX &&
                             fi.PositionY == player.SubPositionY &&
                             fi.FloorItemType == FloorItemType.Item)
                .ToListAsync();

            if (!itemsOnGround.Any()) return BadRequest("Nothing here to pick up.");

            int currentInventoryCount = player.InventoryItems.Count;

            foreach (var floorItem in itemsOnGround)
            {

                if (currentInventoryCount >= player.Capacity) break;

                var instanceId = floorItem.ItemInstanceId;
                if (instanceId == null) continue;


                context.InventoryItems.Add(new InventoryItem
                {
                    PlayerId = id,
                    ItemInstanceId = instanceId.Value,
                    IsInBank = false
                });

                context.FloorItems.Remove(floorItem);
                currentInventoryCount++;
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Database Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return Ok(new { message = "Items picked up.", newCount = currentInventoryCount });
        }

        
    }
}