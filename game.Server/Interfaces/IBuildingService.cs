using game.Server.DTOs;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;

public interface IBuildingService
{
    List<Building> GetCoreBuildings(Guid playerId);
    Task<ActionResult<IEnumerable<BuildingDto>>> GetPlayerBuildingsAsync(Guid playerId, int top, int left, int width, int height);
    Task<ActionResult<FloorDto>> GetFloorByIdAsync(int floorId);
}