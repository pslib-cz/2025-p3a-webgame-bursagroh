using AutoMapper;
using AutoMapper.QueryableExtensions;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Interfaces;
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
        private readonly IErrorService _errorService;

        public BlueprintService(ApplicationDbContext context, IMapper mapper, IErrorService errorService)
        {
            _context = context;
            _mapper = mapper;
            _errorService = errorService;
        }

        public async Task<ActionResult<IEnumerable<BlueprintDto>>> GetBlueprintsWithCraftingsAsync()
        {
            var blueprints = await _context.Blueprints
                .ProjectTo<BlueprintDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new OkObjectResult(blueprints);
        }

        public async Task<ActionResult<IEnumerable<BlueprintDto>>> GetPlayerBlueprintsAsync(Guid playerId)
        {
            var playerExists = await _context.Players.AnyAsync(p => p.PlayerId == playerId);
            if (!playerExists)
                return _errorService.CreateErrorResponse(404, 2001, "Player not found.", "Data Error");

            var ownedBlueprints = await _context.BlueprintPlayers
                .Where(bp => bp.PlayerId == playerId)
                .Select(bp => bp.Blueprint)
                .ProjectTo<BlueprintDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new OkObjectResult(ownedBlueprints);
        }

        public async Task<ActionResult> BuyBlueprintAsync(Guid playerId, int blueprintId)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == playerId);
            var blueprint = await _context.Blueprints.FirstOrDefaultAsync(b => b.BlueprintId == blueprintId);

            if (player == null) {
                return _errorService.CreateErrorResponse(404, 2001, "Player not found.", "Not Found");
            }

            if (player.ScreenType != ScreenTypes.Blacksmith) {
                return _errorService.CreateErrorResponse(400, 2002, "You must be at the Blacksmith to buy blueprints.", "Location Restriction");
            }





            if (blueprint == null) {
                return _errorService.CreateErrorResponse(404, 2003, "Blueprint not found.", "Not Found");
            } 

            var isOwned = await _context.BlueprintPlayers.AnyAsync(bp => bp.PlayerId == playerId && bp.BlueprintId == blueprintId);

            if (isOwned) {
                return _errorService.CreateErrorResponse(400, 2004, "You already own this blueprint.", "Purchase Denied");
            }



            if (player.Money < blueprint.Price) {
                return _errorService.CreateErrorResponse(400, 2005, $"Insufficient funds. Costs {blueprint.Price}, you have {player.Money}.", "Insufficient Gold");
            }
            

            player.Money -= blueprint.Price;
            _context.BlueprintPlayers.Add(new BlueprintPlayer { PlayerId = playerId, BlueprintId = blueprintId });

            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = "Blueprint purchased successfully!", remainingMoney = player.Money });
        }

        public async Task<ActionResult> CraftItemAsync(Guid playerId, int blueprintId)
        {
            var player = await _context.Players
                .Include(p => p.InventoryItems).ThenInclude(ii => ii.ItemInstance)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (player == null) {
                return _errorService.CreateErrorResponse(404, 2001, "Required data not found.", "Not Found");
            }


            if (player.ScreenType != ScreenTypes.Blacksmith) {
                return _errorService.CreateErrorResponse(400, 2002, "You must be at the Blacksmith to buy blueprints.", "Location Restriction");
            }
                

            var blueprint = await _context.Blueprints
                .Include(b => b.Craftings)
                .Include(b => b.Item)
                .FirstOrDefaultAsync(b => b.BlueprintId == blueprintId);

            if (blueprint == null || blueprint.Item == null) {
                return _errorService.CreateErrorResponse(404, 2003, "Required data not found.", "Not Found");
            }
           

            var isOwned = await _context.BlueprintPlayers
                .AnyAsync(bp => bp.PlayerId == playerId && bp.BlueprintId == blueprintId);

            if (!isOwned) {
                return _errorService.CreateErrorResponse(400, 2006, "You do not own this blueprint.", "Crafting Denied");
            }
                

            var inventoryGroups = player.InventoryItems
                .GroupBy(ii => ii.ItemInstance.ItemId)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var req in blueprint.Craftings)
            {
                if (!inventoryGroups.ContainsKey(req.ItemId) || inventoryGroups[req.ItemId] < req.Amount)
                {
                    return _errorService.CreateErrorResponse(400, 2007, $"Missing materials. Need {req.Amount} of ItemID {req.ItemId}.", "Missing Resources");
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
    }
}