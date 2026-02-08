using game.Server.Data;
using game.Server.Models;
using game.Server.Requests;
using game.Server.Types;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _context;
        private readonly MineGenerationService _mineService;

        public CityService(ApplicationDbContext context, MineGenerationService mineService)
        {
            _context = context;
            _mineService = mineService;
        }

        public async Task HandleCityMovement(Player player, MovePlayerRequest request, Guid id)
        {
            int previousX = player.PositionX;
            int previousY = player.PositionY;

            player.PositionX = request.NewPositionX;
            player.PositionY = request.NewPositionY;

            if (player.PositionX == GameConstants.FountainX && player.PositionY == GameConstants.FountainY)
            {
                player.ScreenType = ScreenTypes.Fountain;
            }

            var building = await GetOrGenerateBuilding(player, id);
            if (building != null)
            {
                await ProcessBuildingEntry(player, building, previousX, previousY, id);
            }
        }

        private async Task<Building?> GetOrGenerateBuilding(Player player, Guid id)
        {
            var building = await _context.Buildings.FirstOrDefaultAsync(b =>
                b.PositionX == player.PositionX && b.PositionY == player.PositionY && b.PlayerId == id);

            if (building == null)
            {
                var mapGen = new MapGeneratorService();
                var proceduralArea = mapGen.GenerateMapArea(id, player.PositionX, player.PositionX, player.PositionY, player.PositionY, player.Seed);
                var generatedBuilding = proceduralArea.FirstOrDefault();

                if (generatedBuilding != null)
                {
                    _context.Buildings.Add(generatedBuilding);
                    await _context.SaveChangesAsync();
                    return generatedBuilding;
                }
            }
            return building;
        }

        private async Task ProcessBuildingEntry(Player player, Building building, int previousX, int previousY, Guid id)
        {
            switch (building.BuildingType)
            {
                case BuildingTypes.Abandoned:
                case BuildingTypes.AbandonedTrap:
                    await SetupDungeonEntry(player, building, previousX, previousY);
                    break;
                case BuildingTypes.Mine:
                    await SetupMineEntry(player, building, id);
                    break;
                case BuildingTypes.Bank:
                    player.ScreenType = ScreenTypes.Bank;
                    break;
                case BuildingTypes.Restaurant:
                    player.ScreenType = ScreenTypes.Restaurant;
                    break;
                case BuildingTypes.Blacksmith:
                    player.ScreenType = ScreenTypes.Blacksmith;
                    break;
            }
        }

        private async Task SetupDungeonEntry(Player player, Building building, int previousX, int previousY)
        {
            var floor0 = await _context.Floors.FirstOrDefaultAsync(f => f.BuildingId == building.BuildingId && f.Level == 0);

            if (floor0 == null)
            {
                var mapGen = new MapGeneratorService();
                var generated = mapGen.GenerateInterior(building.BuildingId, player.Seed, 1, building.Height ?? 5, building.PositionX, building.PositionY);
                floor0 = generated.First(f => f.Level == 0);
                _context.Floors.Add(floor0);
                await _context.SaveChangesAsync();
            }

            int spawnX = 0, spawnY = 0;
            if (previousX < building.PositionX) { spawnX = 0; spawnY = 3; }
            else if (previousX > building.PositionX) { spawnX = GameConstants.FloorMaxX; spawnY = 3; }
            else if (previousY < building.PositionY) { spawnX = 3; spawnY = 0; }
            else if (previousY > building.PositionY) { spawnX = 3; spawnY = GameConstants.FloorMaxY; }
            else
            {
                var exit = MapGeneratorService.GetExitCoordinates(building.PositionX, building.PositionY).First();
                spawnX = exit.x; spawnY = exit.y;
            }

            player.FloorId = floor0.FloorId;
            player.ScreenType = ScreenTypes.Floor;
            player.SubPositionX = spawnX;
            player.SubPositionY = spawnY;
        }

        private async Task SetupMineEntry(Player player, Building building, Guid id)
        {
            var playerMine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id);
            player.ScreenType = ScreenTypes.Mine;

            if (playerMine != null)
            {
                _context.Mines.Remove(playerMine);
            }

            playerMine = new Mine { MineId = new Random().Next(), PlayerId = id };
            _context.Mines.Add(playerMine);

            Floor mineFloor = new Floor { BuildingId = building.BuildingId, Level = 0, FloorItems = new List<FloorItem>() };
            _context.Floors.Add(mineFloor);
            await _context.SaveChangesAsync();

            await _mineService.GetOrGenerateLayersBlocksAsync(playerMine.MineId, 1, 5);

            player.FloorId = mineFloor.FloorId;

            player.SubPositionX = GameConstants.MineExitX;
            player.SubPositionY = GameConstants.MineExitY;
        }
    }
}