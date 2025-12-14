using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlueprintController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlueprintController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Blueprint>>> GetBlueprintsWithCraftings()
        {

            List<Blueprint> blueprints = await _context.Blueprints.Include(b => b.Craftings).ToListAsync();

            return Ok(blueprints);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Blueprint>> GetBlueprint(int id)
        {
            Blueprint? blueprint = await _context.Blueprints .Include(b => b.Craftings).FirstOrDefaultAsync(b => b.BlueprintId == id);

            if (blueprint == null)
            {
                return NotFound();
            }

            return Ok(blueprint);
        }
    }
}