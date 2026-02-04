using AutoMapper;
using CrypticWizard.RandomWordGenerator;
using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8620

namespace game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly WordGenerator _generator;

        public SaveController(ApplicationDbContext context, WordGenerator generator)
        {
            _context = context;
            _generator = generator;
        }

        [HttpPost]
        public async Task<ActionResult> ClonePlayerRecord(Guid playerId)
        {
            try
            {
                var originalPlayer = await _context.Players
                    .Include(p => p.InventoryItems).ThenInclude(ii => ii.ItemInstance)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PlayerId == playerId);

                if (originalPlayer == null) return NotFound("Player not found.");

                var clonedPlayer = new Player();
                _context.Entry(clonedPlayer).CurrentValues.SetValues(originalPlayer);
                clonedPlayer.PlayerId = Guid.NewGuid();
                clonedPlayer.Name = originalPlayer.Name;
                clonedPlayer.InventoryItems = new List<InventoryItem>();
                clonedPlayer.FloorId = null;
                clonedPlayer.Floor = null;

                _context.Players.Add(clonedPlayer);
                await _context.SaveChangesAsync();

                var originalBuildings = await _context.Buildings
                    .Include(b => b.Floors).ThenInclude(f => f.FloorItems).ThenInclude(fi => fi.Chest)
                    .Include(b => b.Floors).ThenInclude(f => f.FloorItems).ThenInclude(fi => fi.Enemy)
                    .Include(b => b.Floors).ThenInclude(f => f.FloorItems).ThenInclude(fi => fi.ItemInstance)
                    .Where(b => b.PlayerId == playerId)
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var oldBuilding in originalBuildings)
                {
                    var newBuilding = new Building();
                    _context.Entry(newBuilding).CurrentValues.SetValues(oldBuilding);

                    newBuilding.BuildingId = 0;
                    newBuilding.PlayerId = clonedPlayer.PlayerId;
                    newBuilding.Floors = new List<Floor>();

                    _context.Buildings.Add(newBuilding);
                    await _context.SaveChangesAsync();

                    if (oldBuilding.Floors != null)
                    {
                        foreach (var oldFloor in oldBuilding.Floors)
                        {
                            var newFloor = new Floor
                            {
                                BuildingId = newBuilding.BuildingId,
                                Level = oldFloor.Level,
                                FloorItems = new List<FloorItem>()
                            };

                            _context.Floors.Add(newFloor);
                            await _context.SaveChangesAsync();

                            if (originalPlayer.FloorId == oldFloor.FloorId)
                            {
                                clonedPlayer.FloorId = newFloor.FloorId;
                            }

                            if (oldFloor.FloorItems != null)
                            {
                                foreach (var oldFI in oldFloor.FloorItems)
                                {
                                    var newFI = new FloorItem
                                    {
                                        FloorId = newFloor.FloorId,
                                        PositionX = oldFI.PositionX,
                                        PositionY = oldFI.PositionY,
                                        FloorItemType = oldFI.FloorItemType
                                    };

                                    if (oldFI.Chest != null)
                                    {
                                        newFI.Chest = new Chest();
                                    }

                                    if (oldFI.Enemy != null)
                                    {
                                        newFI.Enemy = new Enemy
                                        {
                                            EnemyType = oldFI.Enemy.EnemyType,
                                            Health = oldFI.Enemy.Health,
                                            ItemInstanceId = oldFI.Enemy.ItemInstanceId
                                        };
                                    }

                                    if (oldFI.ItemInstance != null)
                                    {
                                        newFI.ItemInstance = new ItemInstance
                                        {
                                            ItemId = oldFI.ItemInstance.ItemId,
                                            Durability = oldFI.ItemInstance.Durability
                                        };
                                    }

                                    _context.FloorItems.Add(newFI);
                                }
                            }
                        }
                    }
                }

                var originalMine = await _context.Mines
                    .Include(m => m.MineLayers)
                        .ThenInclude(l => l.MineBlocks)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.PlayerId == playerId);

                if (originalMine != null)
                {
                    var newMine = new Mine
                    {
                        MineId = new Random().Next(),
                        PlayerId = clonedPlayer.PlayerId
                    };
                    _context.Mines.Add(newMine);
                    await _context.SaveChangesAsync();

                    if (originalMine.MineLayers != null)
                    {
                        foreach (var oldLayer in originalMine.MineLayers)
                        {
                            var newLayer = new MineLayer
                            {
                                MineId = newMine.MineId,
                                Depth = oldLayer.Depth
                            };
                            _context.MineLayers.Add(newLayer);
                            await _context.SaveChangesAsync();

                            if (oldLayer.MineBlocks != null)
                            {
                                foreach (var oldBlock in oldLayer.MineBlocks)
                                {
                                    _context.MineBlocks.Add(new MineBlock
                                    {
                                        MineLayerId = newLayer.MineLayerID,
                                        BlockId = oldBlock.BlockId,
                                        Index = oldBlock.Index,
                                        Health = oldBlock.Health
                                    });
                                }
                            }
                        }
                    }
                }

                foreach (var originalInvItem in originalPlayer.InventoryItems)
                {
                    if (originalInvItem.ItemInstance == null) continue;

                    var newInstance = new ItemInstance
                    {
                        ItemId = originalInvItem.ItemInstance.ItemId,
                        Durability = originalInvItem.ItemInstance.Durability
                    };
                    _context.ItemInstances.Add(newInstance);
                    await _context.SaveChangesAsync();

                    _context.InventoryItems.Add(new InventoryItem
                    {
                        PlayerId = clonedPlayer.PlayerId,
                        ItemInstanceId = newInstance.ItemInstanceId,
                        IsInBank = originalInvItem.IsInBank
                    });
                }

                var newSave = new Save
                {
                    PlayerId = clonedPlayer.PlayerId,
                    SaveString = string.Join(" ", _generator.GetWords(WordGenerator.PartOfSpeech.noun, 5))
                };
                _context.Saves.Add(newSave);

                await _context.SaveChangesAsync();

                return Ok(new { newSave.SaveString });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("/api/Load")]
        public async Task<ActionResult> LoadSnapshot(string saveString, Guid targetPlayerId)
        {
            try
            {
                var saveEntry = await _context.Saves.FirstOrDefaultAsync(s => s.SaveString == saveString);
                if (saveEntry == null) return NotFound("Save string not found.");
                var sourceId = saveEntry.PlayerId;


                var sourcePlayer = await _context.Players
                    .Include(p => p.InventoryItems).ThenInclude(ii => ii.ItemInstance)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PlayerId == sourceId);

                if (sourcePlayer == null) return NotFound("Source player not found.");

                var targetPlayer = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == targetPlayerId);
                if (targetPlayer == null) return NotFound("Target player not found.");

                var oldBuildings = await _context.Buildings.Where(b => b.PlayerId == targetPlayerId).ToListAsync();
                _context.Buildings.RemoveRange(oldBuildings);

                var oldMine = await _context.Mines.Where(m => m.PlayerId == targetPlayerId).ToListAsync();
                _context.Mines.RemoveRange(oldMine);

                var oldInv = await _context.InventoryItems.Where(i => i.PlayerId == targetPlayerId).ToListAsync();
                _context.InventoryItems.RemoveRange(oldInv);

                await _context.SaveChangesAsync();
                _context.Entry(targetPlayer).CurrentValues.SetValues(sourcePlayer);
                targetPlayer.PlayerId = targetPlayerId;
                targetPlayer.Name = string.Join(" ", _generator.GetWords(WordGenerator.PartOfSpeech.noun, 1));
                targetPlayer.InventoryItems = new List<InventoryItem>();
                targetPlayer.FloorId = null;

                await _context.SaveChangesAsync();

                var sourceBuildings = await _context.Buildings
                    .Include(b => b.Floors).ThenInclude(f => f.FloorItems).ThenInclude(fi => fi.Chest)
                    .Include(b => b.Floors).ThenInclude(f => f.FloorItems).ThenInclude(fi => fi.Enemy)
                    .Include(b => b.Floors).ThenInclude(f => f.FloorItems).ThenInclude(fi => fi.ItemInstance)
                    .Where(b => b.PlayerId == sourceId)
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var oldBuilding in sourceBuildings)
                {
                    var newBuilding = new Building();
                    _context.Entry(newBuilding).CurrentValues.SetValues(oldBuilding);
                    newBuilding.BuildingId = 0;
                    newBuilding.PlayerId = targetPlayer.PlayerId;
                    newBuilding.Floors = new List<Floor>();

                    _context.Buildings.Add(newBuilding);
                    await _context.SaveChangesAsync();

                    if (oldBuilding.Floors != null)
                    {
                        foreach (var oldFloor in oldBuilding.Floors)
                        {
                            var newFloor = new Floor
                            {
                                BuildingId = newBuilding.BuildingId,
                                Level = oldFloor.Level,
                                FloorItems = new List<FloorItem>()
                            };
                            _context.Floors.Add(newFloor);
                            await _context.SaveChangesAsync();

                            if (sourcePlayer.FloorId == oldFloor.FloorId) targetPlayer.FloorId = newFloor.FloorId;

                            foreach (var oldFI in oldFloor.FloorItems)
                            {
                                var newFI = new FloorItem
                                {
                                    FloorId = newFloor.FloorId,
                                    PositionX = oldFI.PositionX,
                                    PositionY = oldFI.PositionY,
                                    FloorItemType = oldFI.FloorItemType,
                                    Chest = oldFI.Chest != null ? new Chest() : null,
                                    Enemy = oldFI.Enemy != null ? new Enemy
                                    {
                                        EnemyType = oldFI.Enemy.EnemyType,
                                        Health = oldFI.Enemy.Health,
                                        ItemInstanceId = oldFI.Enemy.ItemInstanceId
                                    } : null,
                                    ItemInstance = oldFI.ItemInstance != null ? new ItemInstance
                                    {
                                        ItemId = oldFI.ItemInstance.ItemId,
                                        Durability = oldFI.ItemInstance.Durability
                                    } : null
                                };
                                _context.FloorItems.Add(newFI);
                            }
                        }
                    }
                }

                var sourceMine = await _context.Mines
                    .Include(m => m.MineLayers).ThenInclude(l => l.MineBlocks)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.PlayerId == sourceId);

                if (sourceMine != null)
                {
                    var newMine = new Mine { MineId = new Random().Next(), PlayerId = targetPlayer.PlayerId };
                    _context.Mines.Add(newMine);
                    await _context.SaveChangesAsync();

                    foreach (var oldLayer in sourceMine.MineLayers)
                    {
                        var newLayer = new MineLayer { MineId = newMine.MineId, Depth = oldLayer.Depth };
                        _context.MineLayers.Add(newLayer);
                        await _context.SaveChangesAsync();

                        foreach (var oldBlock in oldLayer.MineBlocks)
                        {
                            _context.MineBlocks.Add(new MineBlock
                            {
                                MineLayerId = newLayer.MineLayerID,
                                BlockId = oldBlock.BlockId,
                                Index = oldBlock.Index,
                                Health = oldBlock.Health
                            });
                        }
                    }
                }

                foreach (var originalInvItem in sourcePlayer.InventoryItems)
                {
                    if (originalInvItem.ItemInstance == null) continue;
                    var newInstance = new ItemInstance
                    {
                        ItemId = originalInvItem.ItemInstance.ItemId,
                        Durability = originalInvItem.ItemInstance.Durability
                    };
                    _context.ItemInstances.Add(newInstance);
                    await _context.SaveChangesAsync();

                    _context.InventoryItems.Add(new InventoryItem
                    {
                        PlayerId = targetPlayer.PlayerId,
                        ItemInstanceId = newInstance.ItemInstanceId,
                        IsInBank = originalInvItem.IsInBank
                    });
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Snapshot loaded", newName = targetPlayer.Name });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}