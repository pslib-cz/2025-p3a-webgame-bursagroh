using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public BankController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<InventoryItem> GetPlayerBankInventory(Guid id)
        {
            IQueryable<InventoryItem> bankItems = context.InventoryItems.Where(i => i.PlayerId == id).Where(i => i.IsInBank == true);

            if (bankItems == null)
            {
                return NoContent();
            }

            List<InventoryItem> items = bankItems.ToList();
            return Ok(items);
        }

        [HttpPatch("{id}/Action/transfer")]
        public async Task<ActionResult<int>> TransferMoney(Guid id, [FromBody] MovePlayerMoneyRequest request)
        {
            if (request.Amount <= 0)
            {
                return BadRequest("amount < 0");
            }

            Player? player = context.Players.Where(i => i.PlayerId == id).FirstOrDefault();

            if (player == null)
            {
                return NotFound();
            }

            int amountToTransfer = request.Amount;
            bool isToBank = request.Direction == Direction.ToBank;

            player.Money += isToBank ? -amountToTransfer : amountToTransfer;
            player.BankBalance += isToBank ? amountToTransfer : -amountToTransfer;

            if (player.Money < 0 || player.BankBalance < 0)
            {
                return BadRequest("insufficient funds");
            }

            await context.SaveChangesAsync();

            return Ok(player.BankBalance);
        }

        [HttpPatch("{id}/Action/move")] // {id} represents the PlayerId
        public async Task<IActionResult> MoveInventoryItem(Guid id, [FromBody] MoveInventoryItemRequest request)
        {
            // 1. Validate Request
            if (request.InventoryItemId <= 0)
            {
                return BadRequest("Invalid InventoryItemId specified.");
            }

            // 2. Find the Inventory Item and Check Ownership
            // Use SingleOrDefaultAsync to find the specific item.
            InventoryItem? item = await context.InventoryItems
                .Where(i => i.InventoryItemId == request.InventoryItemId)
                .SingleOrDefaultAsync();

            if (item == null)
            {
                return NotFound($"Inventory item with ID {request.InventoryItemId} not found.");
            }

            // Crucial security check: Ensure the item belongs to the player making the request
            if (item.PlayerId != id)
            {
                // Return 403 Forbidden or 404 NotFound to prevent enumeration attacks
                return Forbid();
            }

            // 3. Perform the Toggle (NOT operation)
            // Toggling the IsInBank status moves it to the other location.
            item.IsInBank = !item.IsInBank;

            // 4. Save Changes
            // EF Core is tracking the 'item', so saving changes persists the toggle.
            await context.SaveChangesAsync();

            // 5. Return Success
            // Return the updated item to show its new status and location.
            return Ok(item);
        }
    }
}