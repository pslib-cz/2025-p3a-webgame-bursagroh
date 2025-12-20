using game.Server.Data;
using game.Server.Models;
using game.Server.Services; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
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
        public async Task<ActionResult<Floor>> GetSpecificFloor(Guid playerId, int buildingId, int level)
        {
            var player = await _context.Players.FindAsync(playerId);

            if (player == null)
            {
                return NotFound("Player not found.");
            }

            var building = await _context.Buildings.FindAsync(buildingId);

            if (building == null || building.PlayerId != playerId)
            {
                return NotFound("Building not found for this player.");
            }

            if (player.PositionX != building.PositionX || player.PositionY != building.PositionY)
            {
                return BadRequest("Player is not at the building's location.");
            }

            if (player.ScreenType != ScreenTypes.Floor)
            {
                return BadRequest("Player must be in Floor screen to access interior floors.");
            }

            if ((building.BuildingType != BuildingTypes.Abandoned) && (building.BuildingType != BuildingTypes.AbandonedTrap))
            {
                return BadRequest("Interior floors can only be accessed for Abandoned building types.");
            }

            if (building.Height.HasValue && level >= building.Height.Value)
            {
                return BadRequest($"Building only has {building.Height} floors (Levels 0 to {building.Height - 1}). Level {level} is out of bounds.");
            }

            if (level > 0)
            {
                var currentPlayerFloor = await _context.Floors.FindAsync(player.FloorId);

                if (currentPlayerFloor == null || currentPlayerFloor.BuildingId != buildingId || currentPlayerFloor.Level != level - 1)
                {
                    return BadRequest($"Cannot generate floor {level} unless you are standing on floor {level - 1}.");
                }
            }

            var floorsBelowCount = await _context.Floors
                .Where(f => f.BuildingId == buildingId && f.Level < level)
                .CountAsync();

            if (floorsBelowCount < level)
            {
                return BadRequest($"Cannot access floor {level} because floors below it have not been cleared or generated yet.");
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

            int totalHeight = building.Height ?? 5;

            var mapGenerator = new MapGeneratorService();
            int combinedSeed = buildingId + level;

            var generatedFloors = mapGenerator.GenerateInterior(buildingId, combinedSeed, level + 1, totalHeight);
            var newFloor = generatedFloors.FirstOrDefault(f => f.Level == level);

            if (newFloor != null)
            {
                _context.Floors.Add(newFloor);
                building.ReachedHeight = level;

                await _context.SaveChangesAsync();
                return Ok(newFloor);
            }

            return StatusCode(500, "Error generating floor.");
        }
    }
}
