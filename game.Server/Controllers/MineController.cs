using game.Server.Data;
using game.Server.Models;
using game.Server.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MineController : ControllerBase
    {
        private readonly MineService _mineService;
        private readonly ApplicationDbContext context;

        public MineController(MineService mineService, ApplicationDbContext context)
        {
            _mineService = mineService;
            this.context = context;
        }

        [HttpGet("Blocks")]
        public async Task<ActionResult<List<Block>>> GetAllBlocks()
        {
            List<Block> blocks = await context.Blocks.Include(b => b.Item).ToListAsync();

            if (blocks == null || blocks.Count == 0)
            {
                return NotFound();
            }

            return Ok(blocks);
        }

        [HttpPost("Generate")]
        public async Task<IActionResult> GetMine([FromBody] GenerateMineRequest request)
        {
            var player = await context.Players.FirstOrDefaultAsync(p => p.PlayerId == request.PlayerId);

            if (player == null)
            {
                return NotFound($"Player with ID {request.PlayerId} not found.");
            }

            if (player.ScreenType != ScreenTypes.Mine)
            {
                return BadRequest("You can only generate a mine while on the Mine screen.");
            }

            var building = await context.Buildings
                .FirstOrDefaultAsync(b => b.PositionX == player.PositionX &&
                                          b.PositionY == player.PositionY &&
                                          b.BuildingType == BuildingTypes.Mine);

            if (building == null)
            {
                return BadRequest("No Mine building found at your current coordinates.");
            }

            var existingMine = await context.Mines.FirstOrDefaultAsync(m => m.PlayerId == request.PlayerId);
            if (existingMine != null)
            {
                context.Mines.Remove(existingMine);
            }

            Mine mine = new Mine
            {
                MineId = new Random().Next(),
                PlayerId = request.PlayerId
            };
            context.Mines.Add(mine);


            Floor mineFloor = new Floor
            {
                BuildingId = building.BuildingId,
                Level = 0,
                FloorItems = new List<FloorItem>() 
            };

            context.Floors.Add(mineFloor);
            await context.SaveChangesAsync();

            player.FloorId = mineFloor.FloorId;

            player.SubPositionX = 0;
            player.SubPositionY = 0;

            await context.SaveChangesAsync();

            return Ok(new
            {
                Mine = mine,
                Floor = mineFloor,
                Message = "Mine generated and player moved to mine floor."
            });
        }

        [HttpGet("{mineId}/Layer/{layer}")]
        public async Task<ActionResult<List<MineBlock>>> GetLayerBlocks(int mineId, int layer) 
        {
            if (mineId <= 0 || layer < 0)
            {
                return BadRequest();
            }

            try
            {
                var blocks = await _mineService.GetOrGenerateLayersBlocksAsync(mineId, layer);
                return Ok(blocks);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{mineId}/Layers")]
        public async Task<ActionResult<List<MineLayer>>> GetLayerBlocksRange(int mineId, [FromQuery] int startLayer, [FromQuery] int endLayer)
        {
            if (mineId <= 0 || startLayer < 0 || endLayer < 0 || startLayer > endLayer)
            {
                return BadRequest("invalid args");
            }

            try
            {
                var layers = await _mineService.GetOrGenerateLayersBlocksAsync(mineId, startLayer, endLayer);
                return Ok(layers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{PlayerID}/Action/buy")]
        public async Task<ActionResult> BuyCapacity(Guid PlayerID, [FromQuery] int amount)
        {
            var player = await context.Players
                .Include(p => p.InventoryItems)
                    .ThenInclude(ii => ii.ItemInstance)
                        .ThenInclude(ins => ins.Item)
                .FirstOrDefaultAsync(p => p.PlayerId == PlayerID);

            if (player == null)
            {
                return NotFound("Player not found");
            }


            if (player.ScreenType != ScreenTypes.Mine)
            {
                return BadRequest("You can only buy this while at the Mine.");
            }

            bool isThere = (player.SubPositionX == -2 && player.SubPositionY == 1) ||
                                (player.SubPositionX == -2 && player.SubPositionY == 2);

            if (!isThere)
            {
                return BadRequest("You are not at the thing (requires position -2,1 or -2,2).");
            }

            bool alreadyHasPickaxe = player.InventoryItems
                .Any(ii => ii.ItemInstance != null && ii.ItemInstance.ItemId == 30);

            if (alreadyHasPickaxe)
            {
                return BadRequest("You already own a Wooden Pickaxe.");
            }

            const int cost = 5;
            if (player.Money < cost)
            {
                return BadRequest($"Not enough money. Cost is {cost}.");
            }

            var item = await context.Items.FirstOrDefaultAsync(i => i.ItemId == 30);
            if (item == null)
            {
                return BadRequest("Configuration Error: Item 30 does not exist.");
            }

            player.Money -= cost;

            var itemInstance = new ItemInstance
            {
                ItemId = item.ItemId,
                Durability = item.MaxDurability,
                Item = item
            };

            context.ItemInstances.Add(itemInstance);
            await context.SaveChangesAsync();

            var inventoryItem = new InventoryItem
            {
                PlayerId = PlayerID,
                ItemInstanceId = itemInstance.ItemInstanceId,
                ItemInstance = itemInstance,
                IsInBank = false
            };

            context.InventoryItems.Add(inventoryItem);
            player.Capacity += amount;

            await context.SaveChangesAsync();

            return Ok(player);
        }

        /// <remarks>
        /// - mine
        /// - mine layer je y, index x
        /// - hrac musi stat vedle blocku ktery chce tezit
        /// </remarks>
        [HttpPatch("{PlayerID}/Action/mine")]
        public async Task<ActionResult> MineBlock(Guid PlayerID, MineRequest request)
        {
            var player = await context.Players
                .Include(p => p.InventoryItems)
                .FirstOrDefaultAsync(p => p.PlayerId == PlayerID);

            if (player == null) return NotFound("Player not found");

            var chosenItem = await context.InventoryItems
                .Include(ii => ii.ItemInstance)
                    .ThenInclude(ins => ins.Item)
                .FirstOrDefaultAsync(ii => ii.InventoryItemId == request.InventoryItemId && ii.PlayerId == PlayerID);

            if (chosenItem == null) return BadRequest("Item not found in inventory.");
            if (chosenItem.ItemInstance?.Item == null || !chosenItem.ItemInstance.Item.Name.Contains("Pickaxe"))
            {
                return BadRequest("Selected item is not a pickaxe.");
            }
                

            int diffX = Math.Abs(player.SubPositionX - request.TargetX);
            int diffY = Math.Abs(player.SubPositionY - request.TargetY);
            if (!((diffX == 1 && diffY == 0) || (diffX == 0 && diffY == 1)))
                return BadRequest("Target is too far away.");

            var mine = await context.Mines
                .Include(m => m.MineLayers)
                    .ThenInclude(l => l.MineBlocks)
                        .ThenInclude(mb => mb.Block)
                            .ThenInclude(b => b.Item)
                .FirstOrDefaultAsync(m => m.PlayerId == PlayerID);

            if (mine == null) return BadRequest("Mine not found.");

            var targetLayer = mine.MineLayers.FirstOrDefault(l => l.Depth == request.TargetY);
            var targetBlock = targetLayer?.MineBlocks.FirstOrDefault(mb => mb.Index == request.TargetX);

            if (targetBlock == null) return BadRequest("No block at target.");


            targetBlock.Health -= chosenItem.ItemInstance.Item.Damage;

            if (targetBlock.Health > 0)
            {
                await context.SaveChangesAsync();
                return Ok(new { message = "Hit successful", remainingHealth = targetBlock.Health });
            }

            if (!player.FloorId.HasValue) return BadRequest("Player is not on a floor.");

            var random = new Random();
            int amountToGive = random.Next(targetBlock.Block.MinAmount, targetBlock.Block.MaxAmount + 1);

            for (int i = 0; i < amountToGive; i++)
            {
                var itemInstance = new ItemInstance
                {
                    ItemId = targetBlock.Block.ItemId,
                    Durability = targetBlock.Block.Item.MaxDurability
                };
                context.ItemInstances.Add(itemInstance);

                context.FloorItems.Add(new FloorItem
                {
                    FloorId = player.FloorId.Value,
                    PositionX = request.TargetX,
                    PositionY = request.TargetY,
                    FloorItemType = FloorItemType.Item,
                    ItemInstance = itemInstance
                });
            }

            var blockName = targetBlock.Block.BlockType;
            context.MineBlocks.Remove(targetBlock);
            await context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Destroyed {blockName}! Items dropped at ({request.TargetX}, {request.TargetY}).",
                amountDropped = amountToGive
            });
        }
    }
}
