using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Types;
using game.Server.Models;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class NavigationService : INavigationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        private readonly IDungeonService _dungeonService;
        private readonly IBuildingService _buildingService;

        public NavigationService(ApplicationDbContext context, IMapper mapper, ICityService cityService, IDungeonService dungeonService, IBuildingService buildingService)
        {
            _context = context;
            _mapper = mapper;
            _cityService = cityService;
            _dungeonService = dungeonService;
            _buildingService = buildingService;
        }

        public async Task<ActionResult<PlayerDto>> GeneratePlayer(GeneratePlayerRequest request)
        {
            Player player = new Player { Name = request.Name };
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
            if (player == null) return new NotFoundResult();

            var dto = _mapper.Map<PlayerDto>(player);
            var mine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id);
            dto.MineId = mine?.MineId;
            return new OkObjectResult(dto);
        }

        public async Task<ActionResult<PlayerDto>> MoveScreenAsync(Guid id, MoveScreenRequest request)
        {
            var player = await _context.Players.Include(p => p.Floor).FirstOrDefaultAsync(p => p.PlayerId == id);
            if (player == null) return new NotFoundResult();

            player.ScreenType = request.NewScreenType;
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<PlayerDto>(player);
            dto.MineId = (await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id))?.MineId;
            return new OkObjectResult(dto);
        }

        public async Task<ActionResult<PlayerDto>> MovePlayerAsync(Guid id, MovePlayerRequest request)
        {
            var player = await _context.Players.Include(p => p.Floor).Include(p => p.InventoryItems).FirstOrDefaultAsync(p => p.PlayerId == id);
            if (player == null) return new NotFoundResult();

            var playerMine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id);
            int currentX = (player.ScreenType == ScreenTypes.City) ? player.PositionX : player.SubPositionX;
            int currentY = (player.ScreenType == ScreenTypes.City) ? player.PositionY : player.SubPositionY;

            if ((Math.Abs(request.NewPositionX - currentX) + Math.Abs(request.NewPositionY - currentY)) != 1)
                return new BadRequestObjectResult("Move must be exactly 1 square.");

            if (player.ScreenType == ScreenTypes.City)
                await _cityService.HandleCityMovement(player, request, id);
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
    }
}
