using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Types;
using game.Server.Models;
using game.Server.Requests;
using game.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class MineInteractionService : IMineInteractionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly MineGenerationService _generationService;
        private readonly IErrorService _errorService;

        public MineInteractionService(ApplicationDbContext context, IMapper mapper, MineGenerationService generationService, IErrorService errorService)
        {
            _context = context;
            _mapper = mapper;
            _generationService = generationService;
            _errorService = errorService;
        }

        public async Task<ActionResult<List<MineBlockDto>>> GetLayerBlocksAsync(int mineId, int layer)
        {
            if (mineId <= 0 || layer < 0)
            {
                return _errorService.CreateErrorResponse(400, 8004, "Invalid Mine ID or Layer depth.", "Bad Request");
            }

            var blocks = await _generationService.GetOrGenerateLayersBlocksAsync(mineId, layer);
            return new OkObjectResult(_mapper.Map<List<MineBlockDto>>(blocks));
        }

        public async Task<ActionResult<List<MineLayerDto>>> GetLayerBlocksRangeAsync(int mineId, int startLayer, int endLayer)
        {
            if (mineId <= 0 || startLayer > endLayer)
            {
                return _errorService.CreateErrorResponse(400, 8005, "Invalid layer range requested.", "Bad Request");
            }

            var layers = await _generationService.GetOrGenerateLayersBlocksAsync(mineId, startLayer, endLayer);
            return new OkObjectResult(_mapper.Map<List<MineLayerDto>>(layers));
        }

        public async Task<ActionResult<List<MineItemDto>>> GetMineItemsAsync(int mineId)
        {
            var mine = await _context.Mines.FirstOrDefaultAsync(m => m.MineId == mineId);
            if (mine == null)
            {
                return _errorService.CreateErrorResponse(404, 8006, "Mine not found.", "Not Found");
            }

            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == mine.PlayerId);
            if (player?.FloorId == null)
            {
                return _errorService.CreateErrorResponse(400, 8007, "Player is not currently inside a mine floor.", "Action Denied");
            }

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

            if (player == null)
            {
                return _errorService.CreateErrorResponse(404, 8001, "Player not found.", "Not Found");
            }
            if (player.ScreenType != ScreenTypes.Mine)
            {
                return _errorService.CreateErrorResponse(400, 8008, "Must be at the Mine to use the shop.", "Action Denied");
            }

            bool atShop = (player.SubPositionX == GameConstants.PickaxeShopX1 || player.SubPositionX == GameConstants.PickaxeShopX2)
                          && player.SubPositionY == GameConstants.PickaxeShopY;

            if (!atShop)
            {
                return _errorService.CreateErrorResponse(400, 8009, "You are not standing at the pickaxe shop.", "Invalid Location");
            }

            if (player.InventoryItems.Any(ii => ii.ItemInstance?.ItemId == GameConstants.ItemIdWoodenPickaxe))
            {
                return _errorService.CreateErrorResponse(400, 8010, "You already own a Rented Pickaxe.", "Purchase Denied");
            }

            if (player.Money < GameConstants.WoodenPickaxePrice)
            {
                return _errorService.CreateErrorResponse(400, 8011, "Not enough money for the upgrade.", "Insufficient Funds");
            }

            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == GameConstants.ItemIdWoodenPickaxe);
            player.Money -= GameConstants.WoodenPickaxePrice;

            var instance = new ItemInstance { ItemId = GameConstants.ItemIdWoodenPickaxe, Durability = item!.MaxDurability };
            _context.ItemInstances.Add(instance);
            await _context.SaveChangesAsync();

            _context.InventoryItems.Add(new InventoryItem { PlayerId = playerId, ItemInstanceId = instance.ItemInstanceId });

            await _context.SaveChangesAsync();
            return new OkObjectResult(player);
        }

        public async Task<ActionResult> MineBlockAsync(int mineId, MineInteractionRequest request)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var mine = await _context.Mines
                        .Include(m => m.MineLayers).ThenInclude(l => l.MineBlocks).ThenInclude(mb => mb.Block)
                        .FirstOrDefaultAsync(m => m.MineId == mineId);

                    if (mine == null) return _errorService.CreateErrorResponse(404, 8006, "Mine not found.", "Not Found");

                    var player = await _context.Players
                        .Include(p => p.ActiveInventoryItem).ThenInclude(ai => ai.ItemInstance).ThenInclude(ins => ins.Item)
                        .FirstOrDefaultAsync(p => p.PlayerId == mine.PlayerId);

                    var activeItem = player?.ActiveInventoryItem?.ItemInstance?.Item;
                    if (activeItem == null)
                        return _errorService.CreateErrorResponse(400, 8012, "No tool active.", "Equipment Required");

                    int manhattanDistance = Math.Abs(player!.SubPositionX - request.TargetX) + Math.Abs(player.SubPositionY - request.TargetY);
                    if (manhattanDistance != GameConstants.DefaultMiningRange)
                        return _errorService.CreateErrorResponse(400, 8013, "Target too far.", "Range Error");

                    var targetBlock = mine.MineLayers.FirstOrDefault(l => l.Depth == request.TargetY)?
                                            .MineBlocks.FirstOrDefault(mb => mb.Index == request.TargetX);

                    if (targetBlock == null) return _errorService.CreateErrorResponse(404, 8014, "No block found.", "Invalid Target");

                    int finalDamage = activeItem.Damage;
                    bool isAxe = activeItem.Name.Contains("Axe");
                    bool isPickaxe = activeItem.Name.Contains("Pickaxe");
                    bool isWood = targetBlock.Block.ItemId == GameConstants.ItemIdWood;

                    if (isWood && !isAxe)
                    {
                        return _errorService.CreateErrorResponse(400, 8015, "You need an axe to mine wood.", "Wrong Tool");
                    }

                    if (!isWood && isAxe)
                    {
                        return _errorService.CreateErrorResponse(400, 8016, "Axes can only be used on wood.", "Wrong Tool");
                    }

                    if (isAxe && isWood)
                    {
                        finalDamage *= GameConstants.AxeDamageMultiplier;
                    }

                    targetBlock.Health -= finalDamage;

                    if (player?.ActiveInventoryItem?.ItemInstance != null)
                    {
                        player.ActiveInventoryItem.ItemInstance.Durability--;

                        if (player.ActiveInventoryItem.ItemInstance.Durability <= 0)
                        {
                            _context.InventoryItems.Remove(player.ActiveInventoryItem);
                        }
                    }

                    if (targetBlock.Health <= 0)
                    {
                        var random = new Random();
                        int dropAmount = random.Next(targetBlock.Block.MinAmount, targetBlock.Block.MaxAmount + 1);
                        for (int i = 0; i < dropAmount; i++)
                        {
                            _context.FloorItems.Add(new FloorItem
                            {
                                FloorId = player!.FloorId!.Value,
                                PositionX = request.TargetX,
                                PositionY = request.TargetY,
                                ItemInstance = new ItemInstance { ItemId = targetBlock.Block.ItemId }
                            });
                        }
                        _context.MineBlocks.Remove(targetBlock);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new OkObjectResult(new { success = true, destroyed = targetBlock.Health <= 0 });
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
    }
}