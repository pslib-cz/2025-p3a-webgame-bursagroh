using game.Server.DTOs;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;

public interface IBankService
{
    Task<ActionResult<IEnumerable<BankInventoryDto>>> GetPlayerBankInventoryAsync(Guid playerId);
    Task<ActionResult<int>> TransferMoneyAsync(Guid playerId, MovePlayerMoneyRequest request);
    Task<ActionResult> MoveInventoryItemsAsync(Guid playerId, MoveInventoryItemRequest request);
}