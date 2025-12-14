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
        public async Task<ActionResult<IEnumerable<Building>>> GetPlayerBuildings(Guid playerId)
        {
            bool playerExists = await _context.Players.AnyAsync(p => p.PlayerId == playerId);
            if (!playerExists)
            {
                return NotFound();
            }

            List<Building> buildings = await _context.Buildings.Where(b => b.PlayerId == playerId).ToListAsync();

            if (buildings == null || buildings.Count == 0)
            {
                return NotFound();
            }

            return Ok(buildings);
        }
    }
}