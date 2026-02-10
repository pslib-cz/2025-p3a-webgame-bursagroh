using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Types;
using game.Server.Models;
using game.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IErrorService _errorService;

        public InventoryService(ApplicationDbContext context, IMapper mapper, IErrorService errorService)
        {
            _context = context;
            _mapper = mapper;
            _errorService = errorService;
        }

        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetInventoryAsync(Guid id)
        {
            var items = await _context.InventoryItems
                .Where(i => i.PlayerId == id && !i.IsInBank)
                .Include(i => i.ItemInstance).ThenInclude(ins => ins!.Item)
                .ToListAsync();

            return items.Any() ? new OkObjectResult(_mapper.Map<List<InventoryItemDto>>(items)) : new NoContentResult();
        }

        public async Task<ActionResult> PickItemAsync(Guid id, int floorItemId)
        {
            var player = await _context.Players.Include(p => p.InventoryItems).FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) 
            {
                return _errorService.CreateErrorResponse(404, 7001, "Player not found.", "Not Found");
            }

            if (player.FloorId == null) 
            {
                return _errorService.CreateErrorResponse(400, 7002, "Player is not on a floor.", "Action Denied");
            } 

            var itemsOnGround = await _context.FloorItems
                .Where(fi => fi.FloorId == player.FloorId && fi.PositionX == player.SubPositionX &&
                       fi.PositionY == player.SubPositionY && fi.FloorItemId == floorItemId)
                .ToListAsync();

            if (!itemsOnGround.Any()) 
            {
                return _errorService.CreateErrorResponse(400, 7003, "Nothing here to pick up.", "Empty Location");
            }

            int count = player.InventoryItems.Count;
            foreach (var item in itemsOnGround)
            {
                if (count > player.Capacity)
                {
                    return _errorService.CreateErrorResponse(400, 7010, "Your inventory is ful..", "Full Inventory");
                }
                
                if (item.ItemInstanceId == null) continue;

                _context.InventoryItems.Add(new InventoryItem { PlayerId = id, ItemInstanceId = item.ItemInstanceId.Value, IsInBank = false });
                _context.FloorItems.Remove(item);
                count++;
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult(new { message = "Items picked up.", newCount = count });
        }

        public async Task<ActionResult> DropItemAsync(Guid id, int inventoryItemId)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);
            if (player == null) 
            {
                return _errorService.CreateErrorResponse(404, 7001, "Player not found.", "Not Found");
            } 

            var inventoryItem = await _context.InventoryItems.Include(ii => ii.ItemInstance)
                .FirstOrDefaultAsync(ii => ii.InventoryItemId == inventoryItemId && ii.PlayerId == id);

            if (inventoryItem == null) 
            {
                return _errorService.CreateErrorResponse(404, 7004, "Item not found in your inventory.", "Not Found");
            } 
             
            bool isWin = inventoryItem.ItemInstance.ItemId == 100 && player.PositionX == GameConstants.FountainX && player.PositionY == GameConstants.FountainY;
            if (isWin) player.ScreenType = ScreenTypes.Win;
            else if (player.FloorId == null) return _errorService.CreateErrorResponse(400, 7005, "Can only drop items in a floor or mine.", "Action Denied");

            if (player.ActiveInventoryItemId == inventoryItem.InventoryItemId) player.ActiveInventoryItemId = null;

            if (player.FloorId != null)
            {
                _context.FloorItems.Add(new FloorItem
                {
                    FloorId = player.FloorId.Value,
                    PositionX = player.SubPositionX,
                    PositionY = player.SubPositionY,
                    FloorItemType = FloorItemType.Item,
                    ItemInstanceId = inventoryItem.ItemInstanceId
                });
            }

            _context.InventoryItems.Remove(inventoryItem);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = isWin ? "You won!" : "Item dropped.", currentScreen = player.ScreenType.ToString() });
        }

        public async Task<ActionResult<PlayerDto>> SetActiveItemAsync(Guid id, int? inventoryItemId)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);
            if (player == null) 
            {
                return _errorService.CreateErrorResponse(404, 7001, "Player not found.", "Not Found");
            } 

            if (inventoryItemId.HasValue)
            {
                var exists = await _context.InventoryItems.AnyAsync(ii => ii.InventoryItemId == inventoryItemId && ii.PlayerId == id);
                if (!exists) 
                {
                    return _errorService.CreateErrorResponse(400, 7006, "Item not in inventory.", "Invalid Request");
                }
                player.ActiveInventoryItemId = inventoryItemId;
            }
            else player.ActiveInventoryItemId = null;

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<PlayerDto>(player);
            dto.MineId = (await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == id))?.MineId;
            return new OkObjectResult(dto);
        }
    }
}