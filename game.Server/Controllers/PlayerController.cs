using Microsoft.AspNetCore.Mvc;
using game.Server.Models;
using game.Server.Data;

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
        public async Task<Player> Generate([FromBody] GeneratePlayerRequest name)
        {
            Player player = new Player { 
                PlayerId = Guid.NewGuid(), 
                Name = name.Name, 
                Seed = new Random().Next(),
                ScreenType = ScreenTypes.City
            };

            context.Players.Add(player);

            Building building = new Building
            {
                PlayerId = player.PlayerId,
                PositionX = 0,
                PositionY = 0,
                BuildingType = BuildingTypes.Road
            };

            context.Buildings.Add(building);

        
            await context.SaveChangesAsync();

            context.SaveChanges();

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

            player.PositionX = request.NewPositionX;
            player.PositionY = request.NewPositionY;

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