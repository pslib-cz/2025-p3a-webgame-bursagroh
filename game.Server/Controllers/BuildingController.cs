using AutoMapper;
using EFCore.BulkExtensions;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BuildingController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all materialized buildings across all players.
        /// </summary>
        /// <param name="page">The page number (starting at 1).</param>
        /// <param name="pageSize">Number of buildings per page.</param>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<BuildingDto>>> GetAllMaterializedBuildings([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                // Basic validation for pagination
                if (page < 1) page = 1;
                if (pageSize > 200) pageSize = 200; // Cap page size for safety

                var buildings = await _context.Buildings
                    .AsNoTracking()
                    .OrderBy(b => b.PlayerId) // Group by player for readability
                    .ThenBy(b => b.PositionX)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(_mapper.Map<IEnumerable<BuildingDto>>(buildings));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildingDto>>> GetPlayerBuildings(Guid playerId, [FromQuery] int top, [FromQuery] int left, [FromQuery] int width, [FromQuery] int height)
        {
            var player = await _context.Players.AsNoTracking().FirstOrDefaultAsync(p => p.PlayerId == playerId);
            if (player == null) return NotFound();

            int minX = left;
            int maxX = left + width - 1;
            int minY = top;
            int maxY = top + height - 1;

            var coreBuildings = GetCoreBuildings(playerId)
                .Where(b => b.PositionX >= minX && b.PositionX <= maxX && b.PositionY >= minY && b.PositionY <= maxY)
                .ToList();

            var materializedBuildings = await _context.Buildings
                .AsNoTracking()
                .Where(b => b.PlayerId == playerId &&
                            b.PositionX >= minX && b.PositionX <= maxX &&
                            b.PositionY >= minY && b.PositionY <= maxY)
                .ToListAsync();

            var mapGenerator = new MapGeneratorService();
            var proceduralBuildings = mapGenerator.GenerateMapArea(playerId, minX, maxX, minY, maxY, player.Seed);
            var finalMap = new Dictionary<(int, int), Building>();

            foreach (var b in coreBuildings)
            {
                finalMap[(b.PositionX, b.PositionY)] = b;
            }

            foreach (var b in materializedBuildings)
            {
                finalMap[(b.PositionX, b.PositionY)] = b;
            }

            foreach (var b in proceduralBuildings)
            {
                if (!finalMap.ContainsKey((b.PositionX, b.PositionY)))
                {
                    finalMap[(b.PositionX, b.PositionY)] = b;
                }
            }

            var buildingDtos = _mapper.Map<IEnumerable<BuildingDto>>(finalMap.Values);
            return Ok(buildingDtos);
        }
        private List<Building> GetCoreBuildings(Guid playerId) => new List<Building> {
            new Building { PlayerId = playerId, BuildingType = BuildingTypes.Fountain, PositionX = 0, PositionY = 0, IsBossDefeated = false },
            new Building { PlayerId = playerId, BuildingType = BuildingTypes.Mine, PositionX = 2, PositionY = 0, IsBossDefeated = false },
            new Building { PlayerId = playerId, BuildingType = BuildingTypes.Bank, PositionX = -2, PositionY = 0, IsBossDefeated = false },
            new Building { PlayerId = playerId, BuildingType = BuildingTypes.Restaurant, PositionX = 0, PositionY = -2, IsBossDefeated = false },
            new Building { PlayerId = playerId, BuildingType = BuildingTypes.Blacksmith, PositionX = 0, PositionY = 2, IsBossDefeated = false }
        };

        [HttpGet("Floor/{floorId}")]
        public async Task<ActionResult<FloorDto>> GetFloorById(int floorId)
        {
            try
            {
                var floor = await _context.Floors
                .AsNoTracking()
                .Include(f => f.FloorItems!)
                    .ThenInclude(fi => fi.Chest)
                .Include(f => f.FloorItems!)
                    .ThenInclude(fi => fi.Enemy)
                        .ThenInclude(e => e!.ItemInstance)
                            .ThenInclude(ii => ii!.Item)
                .Include(f => f.FloorItems!)
                    .ThenInclude(fi => fi.ItemInstance)
                        .ThenInclude(ii => ii!.Item)
                .FirstOrDefaultAsync(f => f.FloorId == floorId);

                if (floor == null)
                {
                    return NotFound($"Floor with ID {floorId} not found.");
                }

                return Ok(_mapper.Map<FloorDto>(floor));
            } catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }
    }
}
