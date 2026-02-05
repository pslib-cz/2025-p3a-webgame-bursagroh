using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("per_user_limit")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("{id}/SpawnFrames")]
        public async Task<ActionResult> GiveWoodenFrames(Guid id)
            => await SpawnItems(id, 1, 3, "3 Wooden Frames added.");

        [HttpPost("{id}/SpawnSword")]
        public async Task<ActionResult> GiveWoodenSword(Guid id)
            => await SpawnItems(id, "Wooden Sword", 1);

        [HttpPost("{id}/SpawnPickaxe")]
        public async Task<ActionResult> GiveWoodenPickaxe(Guid id)
            => await SpawnItems(id, 30, 1);

        [HttpPost("{id}/SpawnHealingPotion")]
        public async Task<ActionResult> GiveHealingPotion(Guid id)
            => await SpawnItems(id, 40, 1);

        [HttpPost("{id}/SpawnMythicalSword")]
        public async Task<ActionResult> GiveSpecialItem(Guid id)
            => await SpawnItems(id, 100, 1);

        [HttpPost("{id}/FreeMoney")]
        public async Task<ActionResult<Player>> GetFreeMoney(Guid id)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);
            if (player == null) return NotFound();

            player.Money += 10000000;
            await _context.SaveChangesAsync();
            return Ok(player);
        }

        private async Task<ActionResult> SpawnItems(Guid playerId, object itemIdentifier, int count, string? customMessage = null)
        {
            var player = await _context.Players
                .Include(p => p.InventoryItems)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (player == null) return NotFound("Player not found.");

            if (player.InventoryItems.Count + count > player.Capacity)
                return BadRequest($"Inventory full. Need {count} slots, have {player.Capacity - player.InventoryItems.Count}.");

            var template = itemIdentifier switch
            {
                int id => await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id),
                string name => await _context.Items.FirstOrDefaultAsync(i => i.Name == name),
                _ => null
            };

            if (template == null) return BadRequest($"Item template {itemIdentifier} not found.");

            for (int i = 0; i < count; i++)
            {
                var instance = new ItemInstance { ItemId = template.ItemId, Durability = template.MaxDurability };
                _context.ItemInstances.Add(instance);

                _context.InventoryItems.Add(new InventoryItem
                {
                    PlayerId = playerId,
                    ItemInstance = instance,
                    IsInBank = false
                });
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = customMessage ?? $"{template.Name} added to inventory.",
                totalItems = player.InventoryItems.Count,
                itemId = template.ItemId
            });
        }
    }
}