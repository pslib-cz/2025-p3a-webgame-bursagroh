using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetAllItems()
        {
            List<Item> items = await _context.Items.ToListAsync();
            if (items == null || items.Count == 0)
            {
                return NotFound();
            }
            return Ok(items);
        }
    }
}