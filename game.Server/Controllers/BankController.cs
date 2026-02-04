using AutoMapper;
using AutoMapper.QueryableExtensions;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BankController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Inventory")]
        public ActionResult<IEnumerable<BankInventoryDto>> GetPlayerBankInventory(Guid playerId)
        {
            try
            {
                var items = _context.InventoryItems
                .Where(i => i.PlayerId == playerId && i.IsInBank)
                .ProjectTo<BankInventoryDto>(_mapper.ConfigurationProvider)
                .ToList();

                return items.Any() ? Ok(items) : NoContent();
            } catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
            
        }

        [HttpPatch("Action/Transfer")]
        public async Task<ActionResult<int>> TransferMoney(Guid playerId, [FromBody] MovePlayerMoneyRequest request)
        {
            try 
            {
                if (request.Amount <= 0)
                {
                    return BadRequest("amount < 0");
                }

                Player? player = _context.Players.Where(i => i.PlayerId == playerId).FirstOrDefault();

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

                await _context.SaveChangesAsync();

                return Ok(player.BankBalance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
            
        }

        [HttpPatch("Action/Move")]
        public async Task<IActionResult> MoveInventoryItems(Guid playerId, [FromBody] MoveInventoryItemRequest request)
        {
            try
            {
                if (request.InventoryItemIds == null || !request.InventoryItemIds.Any())
                {
                    return BadRequest("No item IDs provided.");
                }

                var items = await _context.InventoryItems
                    .Where(i => i.PlayerId == playerId && request.InventoryItemIds.Contains(i.InventoryItemId))
                    .ToListAsync();

                var player = await _context.Players
                    .FirstOrDefaultAsync(p => p.PlayerId == playerId);

                if (!items.Any() || player == null)
                {
                    return NotFound("No matching items or player found.");
                }

                foreach (var item in items)
                {
                    item.IsInBank = !item.IsInBank;
                    if (item.IsInBank && player.ActiveInventoryItemId == item.InventoryItemId)
                    {
                        player.ActiveInventoryItemId = null;
                    }
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}