using game.Server.DTOs;
using game.Server.Requests;
using game.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("per_user_limit")]
public class PlayerController : ControllerBase
{
    private readonly INavigationService _nav;
    private readonly IInventoryService _inv;
    private readonly ICombatService _combat;

    public PlayerController(INavigationService nav, IInventoryService inv, ICombatService combat)
    {
        _nav = nav;
        _inv = inv;
        _combat = combat;
    }

    [HttpPost("Generate")]
    public async Task<ActionResult<PlayerDto>> Generate([FromBody] GeneratePlayerRequest request) => await _nav.GeneratePlayer(request);

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> GetPlayer(Guid id) => await _nav.GetPlayerAsync(id);

    [HttpPatch("{id}/Action/move-screen")]
    public async Task<ActionResult<PlayerDto>> MoveScreen(Guid id, [FromBody] MoveScreenRequest request) => await _nav.MoveScreenAsync(id, request);

    [HttpPatch("{id}/Action/move")]
    public async Task<ActionResult<PlayerDto>> MovePlayer(Guid id, [FromBody] MovePlayerRequest request) => await _nav.MovePlayerAsync(id, request);

    [HttpGet("{id}/Inventory")]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetPlayerInventory(Guid id) => await _inv.GetInventoryAsync(id);

    [HttpPatch("{id}/Action/pick")]
    public async Task<ActionResult> Pick(Guid id, [FromBody] PickRequest request) => await _inv.PickItemAsync(id, request.FloorItemId);

    [HttpPatch("{id}/Action/drop")]
    public async Task<ActionResult> Drop(Guid id, [FromBody] DropItemRequest request) => await _inv.DropItemAsync(id, request.InventoryItemId);

    [HttpPatch("{id}/Action/set-active-item")]
    public async Task<ActionResult<PlayerDto>> SetActiveItem(Guid id, [FromBody] SetActiveItemRequest request) => await _inv.SetActiveItemAsync(id, request.InventoryItemId);

    [HttpPatch("{id}/Action/use")]
    public async Task<ActionResult> UseItem(Guid id) => await _combat.UseItemAsync(id);

    public class PickRequest { public int FloorItemId { get; set; } }
}