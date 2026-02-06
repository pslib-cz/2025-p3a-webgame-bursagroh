using game.Server.DTOs;
using Microsoft.AspNetCore.Mvc;

public interface IInventoryService
{
    Task<ActionResult<IEnumerable<InventoryItemDto>>> GetInventoryAsync(Guid id);
    Task<ActionResult> PickItemAsync(Guid id, int floorItemId);
    Task<ActionResult> DropItemAsync(Guid id, int inventoryItemId);
    Task<ActionResult<PlayerDto>> SetActiveItemAsync(Guid id, int? inventoryItemId);
}
