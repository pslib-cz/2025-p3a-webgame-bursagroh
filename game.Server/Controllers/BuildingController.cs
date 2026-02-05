using game.Server.DTOs;
using game.Server.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class BuildingController : ControllerBase
{
    private readonly IBuildingService _buildingService;

    public BuildingController(IBuildingService buildingService)
    {
        _buildingService = buildingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BuildingDto>>> GetPlayerBuildings(
        Guid playerId,
        [FromQuery] int top,
        [FromQuery] int left,
        [FromQuery] int width,
        [FromQuery] int height)
        => await _buildingService.GetPlayerBuildingsAsync(playerId, top, left, width, height);

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<BuildingDto>>> GetAllMaterializedBuildings(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
        => await _buildingService.GetAllMaterializedBuildingsAsync(page, pageSize);

    [HttpGet("Floor/{floorId}")]
    public async Task<ActionResult<FloorDto>> GetFloorById(int floorId)
        => await _buildingService.GetFloorByIdAsync(floorId);
}