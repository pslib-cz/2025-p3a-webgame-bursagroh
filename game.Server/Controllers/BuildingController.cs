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
        private static readonly SemaphoreSlim _bulkSemaphore = new SemaphoreSlim(1, 1);
        private readonly IMapper _mapper;

        public BuildingController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildingDto>>> GetPlayerBuildings(Guid playerId, [FromQuery] int top, [FromQuery] int left, [FromQuery] int width, [FromQuery] int height)
        {
            try
            {
                var player = await _context.Players.FindAsync(playerId);
                if (player == null) return NotFound();

                int minX = left;
                int maxX = left + width - 1;
                int minY = top;
                int maxY = top + height - 1;

                var existingBuildings = await _context.Buildings
                    .AsNoTracking()
                    .Where(b => b.PlayerId == playerId &&
                                b.PositionX >= minX && b.PositionX <= maxX &&
                                b.PositionY >= minY && b.PositionY <= maxY)
                    .ToListAsync();

                var mapGenerator = new MapGeneratorService();
                var proceduralBuildings = mapGenerator.GenerateMapArea(playerId, minX, maxX, minY, maxY, player.Seed);

                var existingCoords = new HashSet<(int, int)>(
                    existingBuildings.Select(b => (b.PositionX, b.PositionY))
                );

                var newBuildings = proceduralBuildings
                    .Where(pb => !existingCoords.Contains((pb.PositionX, pb.PositionY)))
                    .ToList();

                if (newBuildings.Any())
                {
                    await _bulkSemaphore.WaitAsync();
                    try
                    {
                        await _context.BulkInsertAsync(newBuildings, config =>
                        {
                            config.SetOutputIdentity = true;
                        });
                    }
                    finally
                    {
                        _bulkSemaphore.Release();
                    }

                    existingBuildings.AddRange(newBuildings);
                }

                var buildingDtos = _mapper.Map<IEnumerable<BuildingDto>>(existingBuildings);
                return Ok(buildingDtos);
            } catch
            {
                return StatusCode(500, "Internal server error.");
            }
        }

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
