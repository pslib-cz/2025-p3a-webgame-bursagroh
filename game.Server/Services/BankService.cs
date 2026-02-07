using AutoMapper;
using AutoMapper.QueryableExtensions;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Interfaces;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class BankService : IBankService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IErrorService _errorService;

        public BankService(ApplicationDbContext context, IMapper mapper, IErrorService errorService)
        {
            _context = context;
            _mapper = mapper;
            _errorService = errorService;
        }

        public async Task<ActionResult<IEnumerable<BankInventoryDto>>> GetPlayerBankInventoryAsync(Guid playerId)
        {
            var items = await _context.InventoryItems
                .Where(i => i.PlayerId == playerId && i.IsInBank)
                .ProjectTo<BankInventoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new OkObjectResult(items);
        }

        public async Task<ActionResult<int>> TransferMoneyAsync(Guid playerId, MovePlayerMoneyRequest request)
        {
            if (request.Amount <= 0)
            {
                return _errorService.CreateErrorResponse(400, 1001, "Amount must be greater than 0", "Invalid Amount");
            }

            var player = await _context.Players.FirstOrDefaultAsync(i => i.PlayerId == playerId);
            if (player == null)
            {
                return _errorService.CreateErrorResponse(404, 1002, "Player record not found.", "Not Found");
            }

            bool isToBank = request.Direction == Direction.ToBank;
            int transferAmount = request.Amount;

            if (isToBank)
            {
                if (player.Money < transferAmount)
                    return _errorService.CreateErrorResponse(400, 1003, "Insufficient wallet funds.", "Transaction Denied");

                player.Money -= transferAmount;
                player.BankBalance += transferAmount;
            }
            else
            {
                if (player.BankBalance < transferAmount)
                    return _errorService.CreateErrorResponse(400, 1004, "Insufficient bank balance.", "Transaction Denied");

                player.BankBalance -= transferAmount;
                player.Money += transferAmount;
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult(player.BankBalance);
        }

        public async Task<ActionResult> MoveInventoryItemsAsync(Guid playerId, MoveInventoryItemRequest request)
        {
            if (request.InventoryItemIds == null || !request.InventoryItemIds.Any())
                return _errorService.CreateErrorResponse(400, 1005, "No item IDs provided.", "Missing Data");

            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == playerId);
            var items = await _context.InventoryItems
                .Where(i => i.PlayerId == playerId && request.InventoryItemIds.Contains(i.InventoryItemId))
                .ToListAsync();

            if (player == null || !items.Any())
                return _errorService.CreateErrorResponse(404, 1006, "No matching items or player found.", "Not Found");

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
    }
}