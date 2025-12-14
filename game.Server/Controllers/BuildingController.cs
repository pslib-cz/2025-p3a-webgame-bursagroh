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
    public class BuildingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BuildingController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("{playerId}")]
        public async Task<ActionResult<IEnumerable<Building>>> GetPlayerBuildings(Guid playerId, [FromQuery] int? top, [FromQuery] int? left, [FromQuery] int? width, [FromQuery] int? height)
        {
            bool playerExists = await _context.Players.AnyAsync(p => p.PlayerId == playerId);
            if (!playerExists)
            {
                return NotFound();
            }

            IQueryable<Building> query = _context.Buildings.Where(b => b.PlayerId == playerId);

            if (top.HasValue && left.HasValue && width.HasValue && height.HasValue)
            {
                int minX = left.Value;
                int maxX = left.Value + width.Value;
                int minY = top.Value;
                int maxY = top.Value + height.Value;

                query = query.Where(b => b.PositionX >= minX && b.PositionX <= maxX && b.PositionY >= minY && b.PositionY <= maxY);
            }

            List<Building> buildings = await query.ToListAsync();
            return Ok(buildings);
        }
    }
}