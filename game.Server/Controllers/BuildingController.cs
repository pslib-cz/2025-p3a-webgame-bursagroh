using game.Server.Data;
using game.Server.Models;
using game.Server.Services; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var playerExists = await _context.Players.AnyAsync(p => p.PlayerId == playerId);
            if (!playerExists) return NotFound();

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

        [HttpGet("{buildingId}/Interior/{level}")]
        public async Task<ActionResult<Floor>> GetSpecificFloor(int buildingId, int level, [FromQuery] Guid playerId)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player == null) return NotFound("Player not found.");

            var floorsBelowCount = await _context.Floors
                .Where(f => f.BuildingId == buildingId && f.Level < level)
                .CountAsync();

            if (floorsBelowCount < level)
            {
                return BadRequest($"Cannot access floor {level}");
            }

            var targetFloor = await _context.Floors
                .Include(f => f.FloorItems!)
                    .ThenInclude(fi => fi.Chest)
                .Include(f => f.FloorItems!)
                    .ThenInclude(fi => fi.Enemy)
                .FirstOrDefaultAsync(f => f.BuildingId == buildingId && f.Level == level);

            if (targetFloor != null)
            {
                return Ok(targetFloor);
            }

            var mapGenerator = new MapGeneratorService();
            int combinedSeed = buildingId + player.Seed;

            var generatedFloors = mapGenerator.GenerateInterior(buildingId, combinedSeed, level + 1);
            var newFloor = generatedFloors.FirstOrDefault(f => f.Level == level);

            if (newFloor != null)
            {
                _context.Floors.Add(newFloor);
                await _context.SaveChangesAsync();
                return Ok(newFloor);
            }

            return StatusCode(500, "Error generating floor.");
        }

        [HttpGet("{buildingId}/Interior")]
        public async Task<ActionResult<IEnumerable<Floor>>> GetBuildingInterior(int buildingId, [FromQuery] Guid playerId, [FromQuery] int floors = 1)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player == null) return NotFound();

            var existingFloors = await _context.Floors
                .Where(f => f.BuildingId == buildingId)
                .Include(f => f.FloorItems!)
                    .ThenInclude(fi => fi.Chest)
                .Include(f => f.FloorItems!)
                    .ThenInclude(fi => fi.Enemy)
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
