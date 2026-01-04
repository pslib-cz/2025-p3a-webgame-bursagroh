using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlueprintController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlueprintController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Blueprint>>> GetBlueprintsWithCraftings()
        {

            List<Blueprint> blueprints = await _context.Blueprints.Include(b => b.Craftings).ToListAsync();

            return Ok(blueprints);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Blueprint>> GetBlueprint(int id)
        {
            Blueprint? blueprint = await _context.Blueprints .Include(b => b.Craftings).FirstOrDefaultAsync(b => b.BlueprintId == id);

            if (blueprint == null)
            {
                return NotFound();
            }

            return Ok(blueprint);
        }

        [HttpGet("Player/{playerId}")]
        public async Task<ActionResult<IEnumerable<Blueprint>>> GetPlayerBlueprints(Guid playerId)
        {
            // 1. Check if the player exists
            var playerExists = await _context.Players.AnyAsync(p => p.PlayerId == playerId);
            if (!playerExists) return NotFound("Player not found.");

            
            var ownedBlueprints = await _context.BlueprintPlayers
                .Where(bp => bp.PlayerId == playerId)
                .Include(bp => bp.Blueprint)
                    .ThenInclude(b => b.Craftings) // Include the recipe details
                .Select(bp => bp.Blueprint)
                .ToListAsync();

            return Ok(ownedBlueprints);
        }

        [HttpPatch("{blueprintId}/Action/buy")]
        public async Task<ActionResult> BuyBlueprint(Guid playerId, int blueprintId)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == playerId);
            var blueprint = await _context.Blueprints.FirstOrDefaultAsync(b => b.BlueprintId == blueprintId);

            if (player.ScreenType != ScreenTypes.Blacksmith)
            {
                return BadRequest("You must be at the Blacksmith to buy blueprints.");
            }

            if (player == null) return NotFound("Player not found.");
            if (blueprint == null) return NotFound("Blueprint not found.");

            var isOwned = await _context.BlueprintPlayers
                .AnyAsync(bp => bp.PlayerId == playerId && bp.BlueprintId == blueprintId);

            if (isOwned)
            {
                return BadRequest("You already own this blueprint.");
            }

            if (player.Money < blueprint.Price)
            {
                return BadRequest($"Insufficient funds. Costs {blueprint.Price}, you have {player.Money}.");
            }

            player.Money -= blueprint.Price;

            var newRecord = new BlueprintPlayer
            {
                PlayerId = playerId,
                BlueprintId = blueprintId
            };

            _context.BlueprintPlayers.Add(newRecord);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Database error during purchase: " + ex.Message);
            }

            return Ok(new
            {
                message = "Blueprint purchased successfully!",
                remainingMoney = player.Money
            });
        }

        [HttpPatch("{blueprintId}/Action/craft")]
        public async Task<ActionResult> CraftItem(Guid playerId, int blueprintId)
        {
            var player = await _context.Players
                .Include(p => p.InventoryItems)
                    .ThenInclude(ii => ii.ItemInstance)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (player.ScreenType != ScreenTypes.Blacksmith)
            {
                return BadRequest("You must be at the Blacksmith to buy blueprints.");
            }

            var blueprint = await _context.Blueprints
                .Include(b => b.Craftings)
                .Include(b => b.Item)
                .FirstOrDefaultAsync(b => b.BlueprintId == blueprintId);

            if (player == null || blueprint == null || blueprint.Item == null)
                return NotFound("Required data not found.");

            var isOwned = await _context.BlueprintPlayers
                .AnyAsync(bp => bp.PlayerId == playerId && bp.BlueprintId == blueprintId);

            if (!isOwned) return BadRequest("You do not own this blueprint.");

    
            var inventoryGroups = player.InventoryItems
                .GroupBy(ii => ii.ItemInstance.ItemId)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var req in blueprint.Craftings)
            {
                if (!inventoryGroups.ContainsKey(req.ItemId) || inventoryGroups[req.ItemId] < req.Amount)
                {
                    return BadRequest($"Missing materials. Need {req.Amount} of ItemID {req.ItemId}.");
                }
            }

            foreach (var req in blueprint.Craftings)
            {
                var itemsToRemove = player.InventoryItems
                    .Where(ii => ii.ItemInstance.ItemId == req.ItemId)
                    .Take(req.Amount)
                    .ToList();

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

            return Ok(new
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