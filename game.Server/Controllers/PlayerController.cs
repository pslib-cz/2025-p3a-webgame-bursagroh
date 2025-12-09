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
        public async Task<Player> Generate([FromBody] string name)
        {
            Player player = new Player { 
                PlayerId = Guid.NewGuid(), 
                Name = name, 
                Seed = new Random().Next() 
            };

            context.Players.Add(player);
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