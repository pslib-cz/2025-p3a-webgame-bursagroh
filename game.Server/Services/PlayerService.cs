using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Types;
using game.Server.Models;
using game.Server.Requests;
using game.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        private readonly IDungeonService _dungeonService;
        private readonly IBuildingService _buildingService;
        private readonly IErrorService _errorService;
        private readonly IMapGeneratorService _mapGeneratorService;

        public PlayerService(ApplicationDbContext context, IMapper mapper, ICityService cityService, IDungeonService dungeonService, IBuildingService buildingService, IErrorService errorService, IMapGeneratorService mapGeneratorService)
        {
            _context = context;
            _mapper = mapper;
            _cityService = cityService;
            _dungeonService = dungeonService;
            _buildingService = buildingService;
            _errorService = errorService;
            _mapGeneratorService = mapGeneratorService;
        }

        public async Task<ActionResult<PlayerDto>> GeneratePlayer(GeneratePlayerRequest request)
        {
            string processedName = (request.Name ?? "").Trim();
            if (processedName.Length > 20)
            {
                processedName = processedName.Substring(0, 20);
            }

            Player player = new Player { Name = processedName, LastModified = DateTime.UtcNow };
            var fixedBuildings = _buildingService.GetCoreBuildings(player.PlayerId);
            Mine mine = new Mine { MineId = new Random().Next(), PlayerId = player.PlayerId };
            player.MineId = mine.MineId;

            _context.Players.Add(player);
            _context.Buildings.AddRange(fixedBuildings);
            _context.Mines.Add(mine);
            await _context.SaveChangesAsync();

            return new OkObjectResult(_mapper.Map<PlayerDto>(player));
        }

        public async Task<ActionResult<PlayerDto>> GetPlayerAsync(Guid id)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) 
            {
                return _errorService.CreateErrorResponse(404, 9001, "Player profile not found.", "Not Found");
            } 

            var dto = _mapper.Map<PlayerDto>(player);
            var mine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id);
            dto.MineId = mine?.MineId;
            return new OkObjectResult(dto);
        }

        public async Task<ActionResult<PlayerDto>> MoveScreenAsync(Guid id, MoveScreenRequest request)
        {
            var player = await _context.Players
                .Include(p => p.Floor)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) 
            {
                return _errorService.CreateErrorResponse(404, 9001, "Player not found.", "Not Found");
            } 

            if (player.ScreenType == ScreenTypes.Lose)
            {
                player.Health = player.MaxHealth;
                player.PositionX = GameConstants.FountainX;
                player.PositionY = GameConstants.FountainY;
                player.SubPositionX = 0;
                player.SubPositionY = 0;
                player.FloorId = null;
            }

            player.ScreenType = request.NewScreenType;

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<PlayerDto>(player);
            dto.MineId = (await _context.Mines.AsNoTracking()
                .FirstOrDefaultAsync(m => m.PlayerId == id))?.MineId;

            return new OkObjectResult(dto);
        }

        public async Task<ActionResult<PlayerDto>> MovePlayerAsync(Guid id, MovePlayerRequest request)
        {
            var player = await _context.Players
                .Include(p => p.Floor)
                .Include(p => p.InventoryItems).ThenInclude(ii => ii.ItemInstance)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) return _errorService.CreateErrorResponse(404, 9001, "Player not found.", "Not Found");

            int currentX = (player.ScreenType == ScreenTypes.City) ? player.PositionX : player.SubPositionX;
            int currentY = (player.ScreenType == ScreenTypes.City) ? player.PositionY : player.SubPositionY;


            var playerMine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id);

            if (player.ScreenType == ScreenTypes.City)
            {
                if (!MapGeneratorService.IsWalkable(request.NewPositionX, request.NewPositionY))
                    return _errorService.CreateErrorResponse(400, 9006, "Void.", "Movement Error");

                await _cityService.HandleCityMovement(player, request, id);
            }
            else
            {
                var result = await _dungeonService.HandleInternalLogic(player, playerMine, request);
                if (result != null) return result;
            }


            await _context.SaveChangesAsync();

            var dto = _mapper.Map<PlayerDto>(player);
            dto.MineId = playerMine?.MineId;
            return new OkObjectResult(dto);
        }

        public async Task<ActionResult<PlayerDto>> RenamePlayerAsync(Guid id, RenamePlayerRequest request)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null)
            {
                return _errorService.CreateErrorResponse(404, 9001, "Player not found.", "Not Found");
            }

            if (string.IsNullOrWhiteSpace(request.NewName))
            {
                return _errorService.CreateErrorResponse(400, 9003, "Name cannot be empty.", "Validation Error");
            }

            string cleanedName = request.NewName.Trim();
            if (cleanedName.Length > 20)
            {
                return _errorService.CreateErrorResponse(400, 9005, "Name is too long. Maximum 20 characters.", "Validation Error");
            }

            if (id.ToString() == GameConstants.ProtectedPlayerId)
            {
                return _errorService.CreateErrorResponse(403, 9004, "This profile identity is protected and cannot be modified.", "Forbidden");
            }

            player.Name = cleanedName;
            player.LastModified = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<PlayerDto>(player);
            var mine = await _context.Mines.AsNoTracking().FirstOrDefaultAsync(m => m.PlayerId == id);
            dto.MineId = mine?.MineId;

            return new OkObjectResult(dto);
        }
    }
}