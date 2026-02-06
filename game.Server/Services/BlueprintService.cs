using AutoMapper;
using AutoMapper.QueryableExtensions;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Models;
using game.Server.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class BlueprintService : IBlueprintService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BlueprintService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<BlueprintDto>>> GetBlueprintsWithCraftingsAsync()
        {
            try
            {
                var blueprints = await _context.Blueprints
                    .ProjectTo<BlueprintDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return new OkObjectResult(blueprints);
            }
            catch (Exception ex)
            {
                return new ObjectResult("Internal server error: " + ex.Message) { StatusCode = 500 };
            }
        }

        public async Task<ActionResult<IEnumerable<BlueprintDto>>> GetPlayerBlueprintsAsync(Guid playerId)
        {
            try
            {
                var playerExists = await _context.Players.AnyAsync(p => p.PlayerId == playerId);
                if (!playerExists) return new NotFoundObjectResult("Player not found.");

                var ownedBlueprints = await _context.BlueprintPlayers
                    .Where(bp => bp.PlayerId == playerId)
                    .Select(bp => bp.Blueprint)
                    .ProjectTo<BlueprintDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new OkObjectResult(ownedBlueprints);
            }
            catch
            {
                return new ObjectResult("Internal server error.") { StatusCode = 500 };
            }
        }

        public async Task<ActionResult> BuyBlueprintAsync(Guid playerId, int blueprintId)
        {
            try
            {
                var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == playerId);
                var blueprint = await _context.Blueprints.FirstOrDefaultAsync(b => b.BlueprintId == blueprintId);

                if (player == null) return new NotFoundObjectResult("Player not found."); 

                if (player.ScreenType != ScreenTypes.Blacksmith)
                    return new BadRequestObjectResult("You must be at the Blacksmith to buy blueprints.");

                
                if (blueprint == null) return new NotFoundObjectResult("Blueprint not found.");

                var isOwned = await _context.BlueprintPlayers
                    .AnyAsync(bp => bp.PlayerId == playerId && bp.BlueprintId == blueprintId);

                if (isOwned) return new BadRequestObjectResult("You already own this blueprint.");

                if (player.Money < blueprint.Price)
                    return new BadRequestObjectResult($"Insufficient funds. Costs {blueprint.Price}, you have {player.Money}.");

                player.Money -= blueprint.Price;
                _context.BlueprintPlayers.Add(new BlueprintPlayer { PlayerId = playerId, BlueprintId = blueprintId });

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    return new BadRequestObjectResult("Database error during purchase: " + ex.Message);
                }

                return new OkObjectResult(new { message = "Blueprint purchased successfully!", remainingMoney = player.Money });
            }
            catch (Exception ex)
            {
                return new ObjectResult("Internal server error: " + ex.Message) { StatusCode = 500 };
            }
        }

        public async Task<ActionResult> CraftItemAsync(Guid playerId, int blueprintId)
        {
            try
            {
                var player = await _context.Players
                    .Include(p => p.InventoryItems).ThenInclude(ii => ii.ItemInstance)
                    .FirstOrDefaultAsync(p => p.PlayerId == playerId);

                if (player == null) return new NotFoundObjectResult("Required data not found.");

                if (player.ScreenType != ScreenTypes.Blacksmith)
                    return new BadRequestObjectResult("You must be at the Blacksmith to buy blueprints.");

                var blueprint = await _context.Blueprints
                    .Include(b => b.Craftings)
                    .Include(b => b.Item)
                    .FirstOrDefaultAsync(b => b.BlueprintId == blueprintId);

                if (blueprint == null || blueprint.Item == null)
                    return new NotFoundObjectResult("Required data not found.");

                var isOwned = await _context.BlueprintPlayers
                    .AnyAsync(bp => bp.PlayerId == playerId && bp.BlueprintId == blueprintId);

                if (!isOwned) return new BadRequestObjectResult("You do not own this blueprint.");

                var inventoryGroups = player.InventoryItems
                    .GroupBy(ii => ii.ItemInstance.ItemId)
                    .ToDictionary(g => g.Key, g => g.Count());

                foreach (var req in blueprint.Craftings)
                {
                    if (!inventoryGroups.ContainsKey(req.ItemId) || inventoryGroups[req.ItemId] < req.Amount)
                    {
                        return new BadRequestObjectResult($"Missing materials. Need {req.Amount} of ItemID {req.ItemId}.");
                    }
                }

                foreach (var req in blueprint.Craftings)
                {
                    var itemsToRemove = player.InventoryItems
                        .Where(ii => ii.ItemInstance.ItemId == req.ItemId)
                        .Take(req.Amount).ToList();

                    foreach (var item in itemsToRemove)
                    {
                        _context.InventoryItems.Remove(item);
                        _context.ItemInstances.Remove(item.ItemInstance);
                    }
                }

                var newItemInstance = new ItemInstance
                {
                    ItemId = blueprint.ItemId,
                    Durability = blueprint.Item.MaxDurability
                };
                _context.ItemInstances.Add(newItemInstance);
                _context.InventoryItems.Add(new InventoryItem
                {
                    PlayerId = playerId,
                    ItemInstance = newItemInstance,
                    IsInBank = false
                });

                await _context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    message = $"Successfully crafted: {blueprint.Item.Name}",
                    stats = new
                    {
                        durability = newItemInstance.Durability,
                        damage = blueprint.Item.Damage
                    }
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult("Internal server error: " + ex.Message) { StatusCode = 500 };
            }
        }
    }
}
