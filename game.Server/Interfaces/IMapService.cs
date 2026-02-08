using game.Server.Models;

public interface IMapGeneratorService
{
    List<Building> GenerateMapArea(Guid playerId, int minX, int maxX, int minY, int maxY, int globalSeed);
    List<Floor> GenerateInterior(int buildingId, int globalSeed, int floorCount, int totalHeight, int bX, int bY);
}