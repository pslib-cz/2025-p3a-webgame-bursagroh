using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Types;
using game.Server.Models;
using game.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IErrorService _errorService;

        public BuildingService(ApplicationDbContext context, IMapper mapper, IErrorService errorService)
        {
            _context = context;
            _mapper = mapper;
            _errorService = errorService;
        }

        public List<Building> GetCoreBuildings(Guid playerId) => new List<Building>
        {
            new Building { PlayerId = playerId, BuildingType = BuildingTypes.Fountain, PositionX = 0, PositionY = 0, IsBossDefeated = false },
            new Building { PlayerId = playerId, BuildingType = BuildingTypes.Mine, PositionX = 2, PositionY = 0, IsBossDefeated = false },
            new Building { PlayerId = playerId, BuildingType = BuildingTypes.Bank, PositionX = -2, PositionY = 0, IsBossDefeated = false },
            new Building { PlayerId = playerId, BuildingType = BuildingTypes.Restaurant, PositionX = 0, PositionY = -2, IsBossDefeated = false },
            new Building { PlayerId = playerId, BuildingType = BuildingTypes.Blacksmith, PositionX = 0, PositionY = 2, IsBossDefeated = false }
        };

        public async Task<ActionResult<IEnumerable<BuildingDto>>> GetPlayerBuildingsAsync(Guid playerId, int top, int left, int width, int height)
        {
            if (width > 20 || height > 20 || width <= 0 || height <= 0) {
                return _errorService.CreateErrorResponse(400, 3001, "Requested area is too large or invalid. Maximum size is 20x20.", "Invalid Request");
            }

            var player = await _context.Players.AsNoTracking().FirstOrDefaultAsync(p => p.PlayerId == playerId);
            if (player == null) 
            {
                return _errorService.CreateErrorResponse(404, 3002, "Player record not found.", "Not Found");
            } 

            int minX = left;
            int maxX = left + width - 1;
            int minY = top;
            int maxY = top + height - 1;

            var finalMap = new Dictionary<(int, int), Building>();
            var coreBuildings = GetCoreBuildings(playerId)
                .Where(b => b.PositionX >= minX && b.PositionX <= maxX && b.PositionY >= minY && b.PositionY <= maxY);

            foreach (var b in coreBuildings)
            {
                finalMap[(b.PositionX, b.PositionY)] = b;
            }

            var materializedBuildings = await _context.Buildings
                .AsNoTracking()
                .Where(b => b.PlayerId == playerId &&
                            b.PositionX >= minX && b.PositionX <= maxX &&
                            b.PositionY >= minY && b.PositionY <= maxY)
                .ToListAsync();

            foreach (var b in materializedBuildings)
            {
                if (!finalMap.ContainsKey((b.PositionX, b.PositionY)))
                {
                    finalMap[(b.PositionX, b.PositionY)] = b;
                }
            }

            var mapGenerator = new MapGeneratorService();
            var proceduralBuildings = mapGenerator.GenerateMapArea(playerId, minX, maxX, minY, maxY, player.Seed);

            foreach (var b in proceduralBuildings)
            {
                if (!finalMap.ContainsKey((b.PositionX, b.PositionY)))
                {
                    finalMap[(b.PositionX, b.PositionY)] = b;
                }
            }

            var buildingDtos = _mapper.Map<IEnumerable<BuildingDto>>(finalMap.Values);
            return new OkObjectResult(buildingDtos);
        }

        public async Task<ActionResult<FloorDto>> GetFloorByIdAsync(int floorId)
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

            if (floor == null) return _errorService.CreateErrorResponse(404, 3003, $"Floor with ID {floorId} not found.", "Not Found");

            return new OkObjectResult(_mapper.Map<FloorDto>(floor));
        }
    }
}