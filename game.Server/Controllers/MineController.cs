using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Models;
using game.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace game.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MineController : ControllerBase
    {
        private readonly MineService _mineService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MineController(MineService mineService, ApplicationDbContext context, IMapper mapper)
        {
            _mineService = mineService;
            _context = context;
            _mapper = mapper;
        }


        [HttpPost("Regenerate")]
        public async Task<IActionResult> GetMine([FromBody] GenerateMineRequest request)
        {
            try
            {
                var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == request.PlayerId);

                if (player == null)
                {
                    return NotFound($"Player with ID {request.PlayerId} not found.");
                }

                if (player.ScreenType != ScreenTypes.Mine)
                {
                    return BadRequest("You can only generate a mine while on the Mine screen.");
                }

                var building = await _context.Buildings
                    .FirstOrDefaultAsync(b => b.PositionX == player.PositionX &&
                                              b.PositionY == player.PositionY &&
                                              b.BuildingType == BuildingTypes.Mine);

                if (building == null)
                {
                    return BadRequest("No Mine building found at your current coordinates.");
                }

                var existingMine = await _context.Mines.FirstOrDefaultAsync(m => m.PlayerId == request.PlayerId);
                if (existingMine != null)
                {
                    _context.Mines.Remove(existingMine);
                    await _context.SaveChangesAsync();
                }

                Mine mine = new Mine
                {
                    MineId = new Random().Next(),
                    PlayerId = request.PlayerId
                };
                _context.Mines.Add(mine);

                Floor mineFloor = new Floor
                {
                    BuildingId = building.BuildingId,
                    Level = 0,
                    FloorItems = new List<FloorItem>()
                };
                _context.Floors.Add(mineFloor);

                await _context.SaveChangesAsync();
                await _mineService.GetOrGenerateLayersBlocksAsync(mine.MineId, 1, 5);


                player.FloorId = mineFloor.FloorId;
                player.MineId = mine.MineId; 
                player.SubPositionX = 0;
                player.SubPositionY = 0;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mine.MineId,
                    Message = "Mine regenerated and player moved to mine floor."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{mineId}/Layer/{layer}")]
        public async Task<ActionResult<List<MineBlockDto>>> GetLayerBlocks(int mineId, int layer)
        {
            try
            {
                if (mineId <= 0 || layer < 0)
                {
                    return BadRequest();
                }

                try
                {
                    var blocks = await _mineService.GetOrGenerateLayersBlocksAsync(mineId, layer);
                    var blockDtos = _mapper.Map<List<MineBlockDto>>(blocks);

                    return Ok(blockDtos);
                }
                catch (InvalidOperationException ex)
                {
                    return NotFound(ex.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpGet("{mineId}/Layers")]
        public async Task<ActionResult<List<MineLayerDto>>> GetLayerBlocksRange(int mineId, [FromQuery] int startLayer, [FromQuery] int endLayer)
        {
            if (mineId <= 0 || startLayer < 0 || endLayer < 0 || startLayer > endLayer)
            {
                return BadRequest("Invalid starting arguments.");
            }

            try
            {
                var layers = await _mineService.GetOrGenerateLayersBlocksAsync(mineId, startLayer, endLayer);
                var layerDtos = _mapper.Map<List<MineLayerDto>>(layers);

                return Ok(layerDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{mineId}/Items")]
        public async Task<ActionResult<List<MineItemDto>>> GetMineItems(int mineId)
        {
            try {
                var mine = await _context.Mines.FirstOrDefaultAsync(m => m.MineId == mineId);

                if (mine == null)
                {
                    return NotFound("Mine not found.");
                }

                var player = await _context.Players
                    .FirstOrDefaultAsync(p => p.PlayerId == mine.PlayerId);

                if (player == null || player.FloorId == null)
                {
                    return BadRequest("Player associated with this mine is not on any floor.");
                }

                var items = await _context.FloorItems
                    .Include(fi => fi.ItemInstance)
                        .ThenInclude(ii => ii!.Item)
                    .Where(fi => fi.FloorId == player.FloorId)
                    .ToListAsync();

                var itemDtos = _mapper.Map<List<MineItemDto>>(items);

                return Ok(itemDtos);
            } 
            catch (Exception ex) 
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPatch("Action/Buy")]
        public async Task<ActionResult> BuyCapacity(Guid playerId, [FromQuery] int amount)
        {

            try
            {
                var player = await _context.Players
                .Include(p => p.InventoryItems)
                    .ThenInclude(ii => ii.ItemInstance)
                        .ThenInclude(ins => ins.Item)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

                if (player == null)
                {
                    return NotFound("Player not found");
                }


                if (player.ScreenType != ScreenTypes.Mine)
                {
                    return BadRequest("You can only buy this while at the Mine.");
                }

                bool isThere = (player.SubPositionX == 1 && player.SubPositionY == -2) ||
                        (player.SubPositionX == 2 && player.SubPositionY == -2);

                if (!isThere)
                {
                    return BadRequest("You are not at the pickaxe thing (requires position 1,-2 or 2,-2).");
                }

                bool alreadyHasPickaxe = player.InventoryItems
                    .Any(ii => ii.ItemInstance != null && ii.ItemInstance.ItemId == 39);

                if (alreadyHasPickaxe)
                {
                    return BadRequest("You already own a Wooden Pickaxe.");
                }

                const int cost = 5;
                if (player.Money < cost)
                {
                    return BadRequest($"Not enough money. Cost is {cost}.");
                }

                var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == 39);
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

                _context.ItemInstances.Add(itemInstance);
                await _context.SaveChangesAsync();

                var inventoryItem = new InventoryItem
                {
                    PlayerId = playerId,
                    ItemInstanceId = itemInstance.ItemInstanceId,
                    ItemInstance = itemInstance,
                    IsInBank = false
                };

                _context.InventoryItems.Add(inventoryItem);
                player.Capacity += amount;

                await _context.SaveChangesAsync();

                return Ok(player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <remarks>
        /// - mine
        /// - mine layer je y, index x
        /// - hrac musi stat vedle blocku ktery chce tezit
        /// </remarks>
        [HttpPatch("{mineId}/Action/Mine")]
        public async Task<ActionResult> MineBlock(int mineId, MineInteractionRequest request)
        {
            try
            {
                var mine = await _context.Mines
                    .Include(m => m.MineLayers)
                        .ThenInclude(l => l.MineBlocks)
                            .ThenInclude(mb => mb.Block)
                                .ThenInclude(b => b.Item)
                    .FirstOrDefaultAsync(m => m.MineId == mineId);

                if (mine == null) return NotFound("Mine not found.");
                var player = await _context.Players
                    .Include(p => p.InventoryItems)
                    .Include(p => p.ActiveInventoryItem)
                        .ThenInclude(ai => ai.ItemInstance)
                            .ThenInclude(ins => ins.Item)
                    .FirstOrDefaultAsync(p => p.PlayerId == mine.PlayerId);

                if (player == null) return NotFound("Player associated with this mine not found.");


                if (player.ActiveInventoryItemId == null || player.ActiveInventoryItem == null)
                {
                    return BadRequest("You don't have an active item selected.");
                }

                var chosenItem = player.ActiveInventoryItem;
                if (chosenItem.ItemInstance?.Item == null || !chosenItem.ItemInstance.Item.Name.Contains("Pickaxe"))
                {
                    return BadRequest("Your active item is not a pickaxe.");
                }


                int diffX = Math.Abs(player.SubPositionX - request.TargetX);
                int diffY = Math.Abs(player.SubPositionY - request.TargetY);
                if (!((diffX == 1 && diffY == 0) || (diffX == 0 && diffY == 1)))
                    return BadRequest("Target is too far away.");


                var targetLayer = mine.MineLayers.FirstOrDefault(l => l.Depth == request.TargetY);
                var targetBlock = targetLayer?.MineBlocks.FirstOrDefault(mb => mb.Index == request.TargetX);

                if (targetBlock == null) return BadRequest("No block at target.");

                targetBlock.Health -= chosenItem.ItemInstance.Item.Damage;
                chosenItem.ItemInstance.Durability -= 1;

                if (chosenItem.ItemInstance.Durability <= 0)
                {
                    player.ActiveInventoryItemId = null;
                    _context.InventoryItems.Remove(chosenItem);
                    _context.ItemInstances.Remove(chosenItem.ItemInstance);
                }

                if (targetBlock.Health > 0)
                {
                    await _context.SaveChangesAsync();
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
                    _context.ItemInstances.Add(itemInstance);

                    _context.FloorItems.Add(new FloorItem
                    {
                        FloorId = player.FloorId.Value,
                        PositionX = request.TargetX,
                        PositionY = request.TargetY,
                        FloorItemType = FloorItemType.Item,
                        ItemInstance = itemInstance
                    });
                }

                var blockName = targetBlock.Block.BlockType;
                _context.MineBlocks.Remove(targetBlock);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = $"Destroyed {blockName}!",
                    name = blockName.ToString(),
                    PositionX = request.TargetX,
                    PositionY = request.TargetY,
                    activeItemId = player.ActiveInventoryItemId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
