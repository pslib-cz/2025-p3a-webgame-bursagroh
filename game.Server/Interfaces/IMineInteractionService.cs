using game.Server.DTOs;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;

public interface IMineInteractionService
{
    Task<ActionResult> RegenerateMineAsync(GenerateMineRequest request);
    Task<ActionResult<List<MineBlockDto>>> GetLayerBlocksAsync(int mineId, int layer);
    Task<ActionResult<List<MineLayerDto>>> GetLayerBlocksRangeAsync(int mineId, int startLayer, int endLayer);
    Task<ActionResult<List<MineItemDto>>> GetMineItemsAsync(int mineId);
    Task<ActionResult> BuyCapacityAsync(Guid playerId, int amount);
    Task<ActionResult> MineBlockAsync(int mineId, MineInteractionRequest request);
}