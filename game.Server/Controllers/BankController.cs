using game.Server.DTOs;
using game.Server.Requests;
using game.Server.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BankController : ControllerBase
{
    private readonly IBankService _bankService;

    public BankController(IBankService bankService) => _bankService = bankService;

    [HttpGet("Inventory")]
    public async Task<ActionResult<IEnumerable<BankInventoryDto>>> GetPlayerBankInventory(Guid playerId)
        => await _bankService.GetPlayerBankInventoryAsync(playerId);

    [HttpPatch("Action/Transfer")]
    public async Task<ActionResult<int>> TransferMoney(Guid playerId, [FromBody] MovePlayerMoneyRequest request)
        => await _bankService.TransferMoneyAsync(playerId, request);

    [HttpPatch("Action/Move")]
    public async Task<IActionResult> MoveInventoryItems(Guid playerId, [FromBody] MoveInventoryItemRequest request)
        => await _bankService.MoveInventoryItemsAsync(playerId, request);
}