using Microsoft.AspNetCore.Mvc;
using game.Server.Models;
using game.Server.Data;

namespace game.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public SessionController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<InventoryItem> GetSaveString(Guid id)
        {
            IQueryable<InventoryItem> bankItems = context.InventoryItems.Where(i => i.PlayerId == id).Where(i => i.IsInBank == true);

            if (bankItems == null)
            {
                return NoContent();
            }

            List<InventoryItem> items = bankItems.ToList();
            return Ok(items);
        }
    }
}