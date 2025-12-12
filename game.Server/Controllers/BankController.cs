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

        [HttpPatch("{id}/Action/move")] 
        public async Task<IActionResult> MoveInventoryItem(Guid id, [FromBody] MoveInventoryItemRequest request)
        {
            if (request.InventoryItemId <= 0)
            {
                return BadRequest();
            }

            InventoryItem? item = await context.InventoryItems
                .Where(i => i.InventoryItemId == request.InventoryItemId)
                .SingleOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            if (item.PlayerId != id)
            {
                return Forbid();
            }

            item.IsInBank = !item.IsInBank;

            await context.SaveChangesAsync();
            return Ok(item);
        }
    }
}