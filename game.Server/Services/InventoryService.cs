using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public interface IInventoryService
    {
        Task<ActionResult<IEnumerable<InventoryItemDto>>> GetInventoryAsync(Guid id);
        Task<ActionResult> PickItemAsync(Guid id, int floorItemId);
        Task<ActionResult> DropItemAsync(Guid id, int inventoryItemId);
        Task<ActionResult<PlayerDto>> SetActiveItemAsync(Guid id, int? inventoryItemId);
    }
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public InventoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            if (player == null) return new NotFoundObjectResult("Player not found.");
            if (player.FloorId == null) return new BadRequestObjectResult("Player is not on a floor.");

            var itemsOnGround = await _context.FloorItems
                .Where(fi => fi.FloorId == player.FloorId && fi.PositionX == player.SubPositionX &&
                       fi.PositionY == player.SubPositionY && fi.FloorItemId == floorItemId && fi.FloorItemType == FloorItemType.Item)
                .ToListAsync();

            if (!itemsOnGround.Any()) return new BadRequestObjectResult("Nothing here to pick up.");

            int count = player.InventoryItems.Count;
            foreach (var item in itemsOnGround)
            {
                if (count >= player.Capacity) break;
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
            if (player == null) return new NotFoundObjectResult("Player not found.");

            var inventoryItem = await _context.InventoryItems.Include(ii => ii.ItemInstance)
                .FirstOrDefaultAsync(ii => ii.InventoryItemId == inventoryItemId && ii.PlayerId == id);

            if (inventoryItem == null) return new BadRequestObjectResult("Item not found.");

            bool isWin = inventoryItem.ItemInstance.ItemId == 100 && player.SubPositionX == 0 && player.SubPositionY == 0;
            if (isWin) player.ScreenType = ScreenTypes.Win;
            else if (player.FloorId == null) return new BadRequestObjectResult("Can only drop in floor/mine.");

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
            if (player == null) return new NotFoundObjectResult("Player not found.");

            if (inventoryItemId.HasValue)
            {
                var exists = await _context.InventoryItems.AnyAsync(ii => ii.InventoryItemId == inventoryItemId && ii.PlayerId == id);
                if (!exists) return new BadRequestObjectResult("Item not in inventory.");
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
