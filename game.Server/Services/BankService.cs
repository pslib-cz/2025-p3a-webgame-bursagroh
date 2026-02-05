using AutoMapper;
using AutoMapper.QueryableExtensions;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class BankService : IBankService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BankService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<BankInventoryDto>>> GetPlayerBankInventoryAsync(Guid playerId)
        {
            try
            {
                var items = await _context.InventoryItems
                    .Where(i => i.PlayerId == playerId && i.IsInBank)
                    .ProjectTo<BankInventoryDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new OkObjectResult(items);
            }
            catch (Exception ex)
            {
                return new ObjectResult("Internal server error: " + ex.Message) { StatusCode = 500 };
            }
        }

        public async Task<ActionResult<int>> TransferMoneyAsync(Guid playerId, MovePlayerMoneyRequest request)
        {
            try
            {
                if (request.Amount <= 0) return new BadRequestObjectResult("Amount must be greater than 0");

                var player = await _context.Players.FirstOrDefaultAsync(i => i.PlayerId == playerId);
                if (player == null) return new NotFoundResult();

                bool isToBank = request.Direction == Direction.ToBank;
                int transferAmount = request.Amount;

                if (isToBank)
                {
                    if (player.Money < transferAmount) return new BadRequestObjectResult("Insufficient wallet funds.");
                    player.Money -= transferAmount;
                    player.BankBalance += transferAmount;
                }
                else
                {
                    if (player.BankBalance < transferAmount) return new BadRequestObjectResult("Insufficient bank balance.");
                    player.BankBalance -= transferAmount;
                    player.Money += transferAmount;
                }

                await _context.SaveChangesAsync();
                return new OkObjectResult(player.BankBalance);
            }
            catch (Exception ex)
            {
                return new ObjectResult("Internal server error: " + ex.Message) { StatusCode = 500 };
            }
        }
        public async Task<ActionResult> MoveInventoryItemsAsync(Guid playerId, MoveInventoryItemRequest request)
        {
            try
            {
                if (request.InventoryItemIds == null || !request.InventoryItemIds.Any())
                    return new BadRequestObjectResult("No item IDs provided.");

                var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == playerId);
                var items = await _context.InventoryItems
                    .Where(i => i.PlayerId == playerId && request.InventoryItemIds.Contains(i.InventoryItemId))
                    .ToListAsync();

                if (player == null || !items.Any())
                    return new NotFoundObjectResult("No matching items or player found.");

                foreach (var item in items)
                {
                    item.IsInBank = !item.IsInBank;

                    if (item.IsInBank && player.ActiveInventoryItemId == item.InventoryItemId)
                    {
                        player.ActiveInventoryItemId = null;
                    }
                }

                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Internal server error: {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}
