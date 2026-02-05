using game.Server.DTOs;
using Microsoft.AspNetCore.Mvc;

public interface IBlueprintService
{
    Task<ActionResult<IEnumerable<BlueprintDto>>> GetBlueprintsWithCraftingsAsync();
    Task<ActionResult<IEnumerable<BlueprintDto>>> GetPlayerBlueprintsAsync(Guid playerId);
    Task<ActionResult> BuyBlueprintAsync(Guid playerId, int blueprintId);
    Task<ActionResult> CraftItemAsync(Guid playerId, int blueprintId);
}