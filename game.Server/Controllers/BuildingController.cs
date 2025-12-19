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
        public async Task<ActionResult<IEnumerable<Building>>> GetPlayerBuildings(Guid playerId, [FromQuery] int top, [FromQuery] int left, [FromQuery] int width, [FromQuery] int height)
        {
            var player = await _context.Players.AnyAsync(p => p.PlayerId == playerId);
            if (!player) return NotFound();

            int minX = left;
            int maxX = left + width;
            int minY = top;
            int maxY = top + height;

            var existingBuildings = await _context.Buildings
                .Where(b => b.PlayerId == playerId &&
                            b.PositionX >= minX && b.PositionX <= maxX &&
                            b.PositionY >= minY && b.PositionY <= maxY)
                .ToListAsync();

            var mapGenerator = new MapGeneratorService();
            var proceduralBuildings = mapGenerator.GenerateMapArea(playerId, minX, maxX, minY, maxY);

            var newBuildings = proceduralBuildings
                .Where(pb => !existingBuildings.Any(eb => eb.PositionX == pb.PositionX && eb.PositionY == pb.PositionY))
                .ToList();

            if (newBuildings.Any())
            {
                _context.Buildings.AddRange(newBuildings);
                await _context.SaveChangesAsync();
                existingBuildings.AddRange(newBuildings);
            }

            return Ok(existingBuildings);
        }

        [HttpGet("{buildingId}/Interior")]
        public async Task<ActionResult<IEnumerable<Floor>>> GetBuildingInterior( int buildingId, [FromQuery] Guid playerId, [FromQuery] int floors = 1)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player == null) return NotFound("Player not found.");

            var existingFloors = await _context.Floors
                .Where(f => f.BuildingId == buildingId)
                .Include(f => f.FloorItems)
                .ToListAsync();

            if (existingFloors.Count < floors)
            {
                var mapGenerator = new MapGeneratorService();
                int seed = buildingId + player.Seed;

                var newFloors = mapGenerator.GenerateInterior(buildingId, seed, floors);
                var floorsToAdd = newFloors.Where(nf => !existingFloors.Any(ef => ef.Level == nf.Level)).ToList();

                if (floorsToAdd.Any())
                {
                    _context.Floors.AddRange(floorsToAdd);
                    await _context.SaveChangesAsync();
                    existingFloors.AddRange(floorsToAdd);
                }
            }

            return Ok(existingFloors.OrderBy(f => f.Level));
        }
    }
}