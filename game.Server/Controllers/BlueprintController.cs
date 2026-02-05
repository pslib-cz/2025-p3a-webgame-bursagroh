using game.Server.DTOs;
using game.Server.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class BlueprintController : ControllerBase
{
    private readonly IBlueprintService _blueprintService;

    public BlueprintController(IBlueprintService blueprintService) => _blueprintService = blueprintService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlueprintDto>>> GetBlueprintsWithCraftings()
        => await _blueprintService.GetBlueprintsWithCraftingsAsync();

    [HttpGet("Player/{playerId}")]
    public async Task<ActionResult<IEnumerable<BlueprintDto>>> GetPlayerBlueprints(Guid playerId)
        => await _blueprintService.GetPlayerBlueprintsAsync(playerId);

    [HttpPatch("{blueprintId}/Action/buy")]
    public async Task<ActionResult> BuyBlueprint(Guid playerId, int blueprintId)
        => await _blueprintService.BuyBlueprintAsync(playerId, blueprintId);

    [HttpPatch("{blueprintId}/Action/craft")]
    public async Task<ActionResult> CraftItem(Guid playerId, int blueprintId)
        => await _blueprintService.CraftItemAsync(playerId, blueprintId);
}