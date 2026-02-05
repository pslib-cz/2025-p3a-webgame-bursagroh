using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Types;
using game.Server.Models;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class MineInteractionService : IMineInteractionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly MineGenerationService _generationService;

        public MineInteractionService(ApplicationDbContext context, IMapper mapper, MineGenerationService generationService)
        {
            _context = context;
            _mapper = mapper;
            _generationService = generationService;
        }

        public async Task<ActionResult> RegenerateMineAsync(GenerateMineRequest request)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == request.PlayerId);
            if (player == null) return new NotFoundObjectResult($"Player {request.PlayerId} not found.");
            if (player.ScreenType != ScreenTypes.Mine) return new BadRequestObjectResult("Must be on Mine screen.");

            var building = await _context.Buildings.FirstOrDefaultAsync(b =>
                b.PositionX == player.PositionX && b.PositionY == player.PositionY && b.BuildingType == BuildingTypes.Mine);
            if (building == null) return new BadRequestObjectResult("No Mine building at coordinates.");

            var existingMine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == request.PlayerId);
            if (existingMine != null) _context.Mines.Remove(existingMine);

            var mine = new Mine { MineId = new Random().Next(), PlayerId = request.PlayerId };
            var mineFloor = new Floor { BuildingId = building.BuildingId, Level = 0, FloorItems = new List<FloorItem>() };

            _context.Mines.Add(mine);
            _context.Floors.Add(mineFloor);
            await _context.SaveChangesAsync();

            await _generationService.GetOrGenerateLayersBlocksAsync(mine.MineId, 1, 5);

            player.FloorId = mineFloor.FloorId;
            player.MineId = mine.MineId;
            player.SubPositionX = 0;
            player.SubPositionY = 0;

            await _context.SaveChangesAsync();
            return new OkObjectResult(new { mine.MineId, Message = "Mine regenerated." });
        }

        public async Task<ActionResult<List<MineBlockDto>>> GetLayerBlocksAsync(int mineId, int layer)
        {
            if (mineId <= 0 || layer < 0) return new BadRequestResult();
            var blocks = await _generationService.GetOrGenerateLayersBlocksAsync(mineId, layer);
            return new OkObjectResult(_mapper.Map<List<MineBlockDto>>(blocks));
        }

        public async Task<ActionResult<List<MineLayerDto>>> GetLayerBlocksRangeAsync(int mineId, int startLayer, int endLayer)
        {
            if (mineId <= 0 || startLayer > endLayer) return new BadRequestObjectResult("Invalid arguments.");
            var layers = await _generationService.GetOrGenerateLayersBlocksAsync(mineId, startLayer, endLayer);
            return new OkObjectResult(_mapper.Map<List<MineLayerDto>>(layers));
        }

        public async Task<ActionResult<List<MineItemDto>>> GetMineItemsAsync(int mineId)
        {
            var mine = await _context.Mines.FirstOrDefaultAsync(m => m.MineId == mineId);
            if (mine == null) return new NotFoundObjectResult("Mine not found.");

            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == mine.PlayerId);
            if (player?.FloorId == null) return new BadRequestObjectResult("Player not on a floor.");

            var items = await _context.FloorItems
                .Include(fi => fi.ItemInstance).ThenInclude(ii => ii!.Item)
                .Where(fi => fi.FloorId == player.FloorId).ToListAsync();

            return new OkObjectResult(_mapper.Map<List<MineItemDto>>(items));
        }

        public async Task<ActionResult> BuyCapacityAsync(Guid playerId, int amount)
        {
            var player = await _context.Players
                .Include(p => p.InventoryItems).ThenInclude(ii => ii.ItemInstance).ThenInclude(ins => ins.Item)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (player == null) return new NotFoundObjectResult("Player not found");
            if (player.ScreenType != ScreenTypes.Mine) return new BadRequestObjectResult("Must be at Mine.");

            if (!((player.SubPositionX == 1 || player.SubPositionX == 2) && player.SubPositionY == -2))
                return new BadRequestObjectResult("Not at the pickaxe shop.");

            if (player.InventoryItems.Any(ii => ii.ItemInstance?.ItemId == 39))
                return new BadRequestObjectResult("Already own a Wooden Pickaxe.");

            if (player.Money < 5) return new BadRequestObjectResult("Not enough money.");

            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == 39);
            player.Money -= 5;

            var instance = new ItemInstance { ItemId = 39, Durability = item.MaxDurability };
            _context.ItemInstances.Add(instance);
            await _context.SaveChangesAsync();

            _context.InventoryItems.Add(new InventoryItem { PlayerId = playerId, ItemInstanceId = instance.ItemInstanceId });
            player.Capacity += amount;

            await _context.SaveChangesAsync();
            return new OkObjectResult(player);
        }

        public async Task<ActionResult> MineBlockAsync(int mineId, MineInteractionRequest request)
        {
            var mine = await _context.Mines
                .Include(m => m.MineLayers).ThenInclude(l => l.MineBlocks).ThenInclude(mb => mb.Block).ThenInclude(b => b.Item)
                .FirstOrDefaultAsync(m => m.MineId == mineId);

            var player = await _context.Players
                .Include(p => p.ActiveInventoryItem).ThenInclude(ai => ai.ItemInstance).ThenInclude(ins => ins.Item)
                .FirstOrDefaultAsync(p => p.PlayerId == mine.PlayerId);

            if (player?.ActiveInventoryItem?.ItemInstance?.Item == null || !player.ActiveInventoryItem.ItemInstance.Item.Name.Contains("Pickaxe"))
                return new BadRequestObjectResult("No pickaxe active.");

            if (Math.Abs(player.SubPositionX - request.TargetX) + Math.Abs(player.SubPositionY - request.TargetY) != 1)
                return new BadRequestObjectResult("Target too far.");

            var targetBlock = mine.MineLayers.FirstOrDefault(l => l.Depth == request.TargetY)?
                                  .MineBlocks.FirstOrDefault(mb => mb.Index == request.TargetX);

            if (targetBlock == null) return new BadRequestObjectResult("No block.");

            targetBlock.Health -= player.ActiveInventoryItem.ItemInstance.Item.Damage;
            player.ActiveInventoryItem.ItemInstance.Durability--;

            if (player.ActiveInventoryItem.ItemInstance.Durability <= 0) _context.InventoryItems.Remove(player.ActiveInventoryItem);

            if (targetBlock.Health > 0) { await _context.SaveChangesAsync(); return new OkObjectResult(new { hit = true }); }

            var random = new Random();
            int amount = random.Next(targetBlock.Block.MinAmount, targetBlock.Block.MaxAmount + 1);
            for (int i = 0; i < amount; i++)
            {
                _context.FloorItems.Add(new FloorItem
                {
                    FloorId = player.FloorId.Value,
                    PositionX = request.TargetX,
                    PositionY = request.TargetY,
                    ItemInstance = new ItemInstance { ItemId = targetBlock.Block.ItemId }
                });
            }

            _context.MineBlocks.Remove(targetBlock);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new { destroyed = true });
        }
    }
}
