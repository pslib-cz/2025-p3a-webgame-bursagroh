using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                Seed = new Random().Next()
            };

            Building building = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Road,
                PositionX = 0,
                PositionY = 0,
                IsBossDefeated = false
            };

            context.Players.Add(player);
            context.Buildings.Add(building);
            await context.SaveChangesAsync();

            Floor floor = new Floor
            {
                BuildingId = building.BuildingId, 
                Level = 1
            };

            context.Floors.Add(floor);
            await context.SaveChangesAsync();

            FloorItem floorItem = new FloorItem
            {
                FloorId = floor.FloorId,
                PositionX = 0,
                PositionY = 0,
                FloorItemType = FloorItemType.Player
            };

            context.FloorItems.Add(floorItem);
            await context.SaveChangesAsync();

            player.BuildingId = building.BuildingId;
            player.FloorItemId = floorItem.FloorItemId;

            context.Players.Update(player);
            await context.SaveChangesAsync();

            return player;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(Guid id) {
            Player? player = await context.Players.FindAsync(id);

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

        [HttpPatch("{id}/Action/move-screen")]
        public async Task<ActionResult> MoveScreen(Guid id, [FromBody] MoveScreenRequest newScreenType)
        {
            Player? player = await context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            if (player.ScreenType == newScreenType.NewScreenType)
            {
                return Ok(player);
            }

            player.ScreenType = newScreenType.NewScreenType;
            await context.SaveChangesAsync();

            return Ok(player);
        }

        [HttpPatch("{id}/Action/move")]
        public async Task<ActionResult> MovePlayer(Guid id, [FromBody] MovePlayerRequest request)
        {
            Player? player = await context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            FloorItem? currentPositionItem = await context.FloorItems.Where(fi => fi.FloorItemId == player.FloorItemId).FirstOrDefaultAsync();

            if (currentPositionItem == null)
            {
                return NotFound();
            }

            if (currentPositionItem.PositionX == request.NewPositionX && currentPositionItem.PositionY == request.NewPositionY)
            {
                return Ok(currentPositionItem);
            }

            bool isAdjacent = (Math.Abs(request.NewPositionX - currentPositionItem.PositionX) +
                       Math.Abs(request.NewPositionY - currentPositionItem.PositionY)) == 1; //ukradeno

            if (!isAdjacent)
            {
                return BadRequest("move > 1");
            }

            currentPositionItem.PositionX = request.NewPositionX;
            currentPositionItem.PositionY = request.NewPositionY;

            await context.SaveChangesAsync();
            return Ok(player);
        }

        [HttpGet("{id}/Inventory")]
        public ActionResult<InventoryItem> GetPlayerInventory(Guid id)
        {
            IQueryable<InventoryItem> inventoryItems = context.InventoryItems.Where(i => i.PlayerId == id).Where(i => i.IsInBank == false);

            if (inventoryItems == null)
            {
                return NoContent();
            }

            List<InventoryItem> items = inventoryItems.ToList();
            return Ok(items);
        }

    }
}