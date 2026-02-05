using game.Server.DTOs;
using game.Server.Requests;
using game.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("per_user_limit")]
public class MineController : ControllerBase
{
    private readonly IMineInteractionService _mineService;

    public MineController(IMineInteractionService mineService)
    {
        _mineService = mineService;
    }

    [HttpPost("Regenerate")]
    public async Task<IActionResult> GetMine([FromBody] GenerateMineRequest request)
        => await _mineService.RegenerateMineAsync(request);

    [HttpGet("{mineId}/Layer/{layer}")]
    public async Task<ActionResult<List<MineBlockDto>>> GetLayerBlocks(int mineId, int layer)
        => await _mineService.GetLayerBlocksAsync(mineId, layer);

    [HttpGet("{mineId}/Layers")]
    public async Task<ActionResult<List<MineLayerDto>>> GetLayerBlocksRange(int mineId, [FromQuery] int startLayer, [FromQuery] int endLayer)
        => await _mineService.GetLayerBlocksRangeAsync(mineId, startLayer, endLayer);

    [HttpGet("{mineId}/Items")]
    public async Task<ActionResult<List<MineItemDto>>> GetMineItems(int mineId)
        => await _mineService.GetMineItemsAsync(mineId);

    [HttpPatch("Action/Buy")]
    public async Task<ActionResult> BuyCapacity(Guid playerId, [FromQuery] int amount)
        => await _mineService.BuyCapacityAsync(playerId, amount);

    [HttpPatch("{mineId}/Action/Mine")]
    public async Task<ActionResult> MineBlock(int mineId, MineInteractionRequest request)
        => await _mineService.MineBlockAsync(mineId, request);
}