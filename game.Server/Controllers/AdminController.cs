using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly ApplicationDbContext context;

        
        public AdminController(ApplicationDbContext context)
        {
           this.context = context;
        }

        [HttpPost("{id}/SpawnFrames")]
        public async Task<ActionResult> GiveWoodenFrames(Guid id)
        {

            var player = await context.Players
                .Include(p => p.InventoryItems)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) return NotFound("Player not found.");


            if (player.InventoryItems.Count + 3 > player.Capacity)
            {
                return BadRequest($"Not enough space! You need 3 slots, but only have {player.Capacity - player.InventoryItems.Count} left.");
            }

            var frameTemplate = await context.Items
                .FirstOrDefaultAsync(i => i.Name == "Wooden Frame" || i.ItemId == 1);

            if (frameTemplate == null)
            {
                return BadRequest("Wooden Frame item template not found in database.");
            }

            for (int i = 0; i < 3; i++)
            {
                var newInstance = new ItemInstance
                {
                    ItemId = frameTemplate.ItemId,
                    Durability = frameTemplate.MaxDurability
                };
                context.ItemInstances.Add(newInstance);

                var inventoryEntry = new InventoryItem
                {
                    PlayerId = id,
                    ItemInstance = newInstance,
                    IsInBank = false
                };
                context.InventoryItems.Add(inventoryEntry);
            }

            await context.SaveChangesAsync();

            return Ok(new
            {
                message = "3 Wooden Frames added to inventory.",
                totalInventoryCount = player.InventoryItems.Count
            });
        }

        [HttpPost("{id}/SpawnSword")]
        public async Task<ActionResult> GiveWoodenSword(Guid id)
        {
            var player = await context.Players
                .Include(p => p.InventoryItems)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) return NotFound("Player not found.");

            if (player.InventoryItems.Count >= player.Capacity)
            {
                return BadRequest("full");
            }


            var pickaxeTemplate = await context.Items
                .FirstOrDefaultAsync(i => i.Name == "Wooden Sword");

            if (pickaxeTemplate == null)
            {
                return BadRequest("not found");
            }

            var newInstance = new ItemInstance
            {
                ItemId = pickaxeTemplate.ItemId,
                Durability = pickaxeTemplate.MaxDurability
            };
            context.ItemInstances.Add(newInstance);
            var inventoryEntry = new InventoryItem
            {
                PlayerId = id,
                ItemInstance = newInstance,
                IsInBank = false
            };
            context.InventoryItems.Add(inventoryEntry);

            await context.SaveChangesAsync();

            return Ok(new
            {
                message = "Wooden Sword added to inventory!",
                inventoryItemId = inventoryEntry.InventoryItemId,
                stats = new
                {
                    damage = pickaxeTemplate.Damage,
                    durability = pickaxeTemplate.MaxDurability
                }
            });
        }

        [HttpPost("{id}/SpawnPickaxe")]
        public async Task<ActionResult> GiveWoodenPickaxe(Guid id)
        {
            var player = await context.Players
                .Include(p => p.InventoryItems)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) return NotFound("Player not found.");
            if (player.InventoryItems.Count >= player.Capacity)
            {
                return BadRequest("Inventory is full.");
            }

            var pickaxeTemplate = await context.Items
                .FirstOrDefaultAsync(i => i.ItemId == 30);

            var newInstance = new ItemInstance
            {
                ItemId = pickaxeTemplate.ItemId,
                Durability = pickaxeTemplate.MaxDurability
            };
            context.ItemInstances.Add(newInstance);

            var inventoryEntry = new InventoryItem
            {
                PlayerId = id,
                ItemInstance = newInstance,
                IsInBank = false
            };
            context.InventoryItems.Add(inventoryEntry);

            await context.SaveChangesAsync();

            return Ok(new
            {
                message = "Wooden Pickaxe added to inventory!",
                inventoryItemId = inventoryEntry.InventoryItemId,
                stats = new
                {
                    damage = pickaxeTemplate.Damage,
                    durability = pickaxeTemplate.MaxDurability
                }
            });
        }

        [HttpPost("{id}/FreeMoney")]
        /// <remarks>
        /// - test purposes
        /// - v production odstranit
        /// </remarks>
        public async Task<ActionResult<Player>> GetFreeMoney(Guid id)
        {
            Player? player = await context.Players.Where(p => p.PlayerId == id).FirstOrDefaultAsync(p => p.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }
            player.Money += 10000000;
            await context.SaveChangesAsync();
            return Ok(player);
        }

        [HttpPost("{id}/SpawnHealingPotion")]
        public async Task<ActionResult> GiveHealingPotion(Guid id)
        {
            // 1. Fetch player and check existence
            var player = await context.Players
                .Include(p => p.InventoryItems)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) return NotFound("Player not found.");

            if (player.InventoryItems.Count >= player.Capacity)
            {
                return BadRequest("Inventory is full.");
            }


            var potionTemplate = await context.Items
                .FirstOrDefaultAsync(i => i.ItemId == 40);

            if (potionTemplate == null)
            {
                return BadRequest("Healing Potion template (ID 40) not found in database.");
            }

            var newInstance = new ItemInstance
            {
                ItemId = potionTemplate.ItemId,
                Durability = potionTemplate.MaxDurability
            };
            context.ItemInstances.Add(newInstance);

            var inventoryEntry = new InventoryItem
            {
                PlayerId = id,
                ItemInstance = newInstance,
                IsInBank = false
            };
            context.InventoryItems.Add(inventoryEntry);


            await context.SaveChangesAsync();

            return Ok(new
            {
                message = "Healing Potion added to inventory!",
                inventoryItemId = inventoryEntry.InventoryItemId,
                itemName = potionTemplate.Name
            });
        }

        [HttpPost("{id}/SpawnItem100")]
        public async Task<ActionResult> GiveSpecialItem(Guid id)
        {
            // 1. Fetch player and check inventory capacity
            var player = await context.Players
                .Include(p => p.InventoryItems)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) return NotFound("Player not found.");

            if (player.InventoryItems.Count >= player.Capacity)
            {
                return BadRequest("Inventory is full.");
            }

            // 2. Fetch the template for Item 100
            var itemTemplate = await context.Items
                .FirstOrDefaultAsync(i => i.ItemId == 100);

            if (itemTemplate == null)
            {
                return BadRequest("Item template ID 100 not found in database.");
            }

            // 3. Create the instance and link it to the player's inventory
            var newInstance = new ItemInstance
            {
                ItemId = itemTemplate.ItemId,
                Durability = itemTemplate.MaxDurability
            };
            context.ItemInstances.Add(newInstance);

            var inventoryEntry = new InventoryItem
            {
                PlayerId = id,
                ItemInstance = newInstance,
                IsInBank = false
            };
            context.InventoryItems.Add(inventoryEntry);

            // 4. Persist changes
            await context.SaveChangesAsync();

            return Ok(new
            {
                message = $"{itemTemplate.Name} added to inventory!",
                inventoryItemId = inventoryEntry.InventoryItemId,
                itemId = itemTemplate.ItemId
            });
        }
    }
}
