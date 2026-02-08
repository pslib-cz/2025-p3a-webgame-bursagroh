using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Types;
using game.Server.Models;
using game.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class CombatService : ICombatService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IErrorService _errorService;

        public CombatService(ApplicationDbContext context, IMapper mapper, IErrorService errorService)
        {
            _context = context;
            _mapper = mapper;
            _errorService = errorService;
        }

        public async Task<ActionResult> UseItemAsync(Guid id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var player = await _context.Players
                    .Include(p => p.ActiveInventoryItem).ThenInclude(ai => ai.ItemInstance).ThenInclude(ii => ii.Item)
                    .Include(p => p.InventoryItems)
                    .FirstOrDefaultAsync(p => p.PlayerId == id);

                if (player == null)
                {
                    return _errorService.CreateErrorResponse(404, 5001, "Player not found.", "Not Found");
                }

                ItemInstance? activeInstance = player.ActiveInventoryItem?.ItemInstance;
                Item? itemData = activeInstance?.Item;

                bool isWeapon = itemData == null ||
                               itemData.ItemType == ItemTypes.Sword ||
                               itemData.ItemType == ItemTypes.Axe ||
                               itemData.ItemType == ItemTypes.Pickaxe;

                if (isWeapon)
                {
                    if (player.ScreenType != ScreenTypes.Fight)
                    {
                        return _errorService.CreateErrorResponse(400, 5002, "You can only attack during a fight.", "Combat Denied");
                    }

                    var floorItem = await _context.FloorItems
                        .Include(fi => fi.Enemy)
                        .FirstOrDefaultAsync(fi => fi.FloorId == player.FloorId &&
                                                   fi.PositionX == player.SubPositionX &&
                                                   fi.PositionY == player.SubPositionY);

                    if (floorItem?.Enemy == null)
                    {
                        player.ScreenType = ScreenTypes.Floor;
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return _errorService.CreateErrorResponse(400, 5003, "No enemy found here.", "Target Lost");
                    }

                    var enemy = floorItem.Enemy;
                    var rng = new Random();

                    int playerDamage = itemData?.Damage ?? 1;
                    enemy.Health -= playerDamage;

                    if (activeInstance != null)
                    {
                        activeInstance.Durability -= 1;
                        if (activeInstance.Durability <= 0)
                        {
                            var activeInvItem = player.ActiveInventoryItem!;
                            player.ActiveInventoryItemId = null;
                            _context.InventoryItems.Remove(activeInvItem);
                            _context.ItemInstances.Remove(activeInstance);
                        }
                    }

                    if (enemy.Health > 0)
                    {
                        if (rng.NextDouble() < GameConstants.EnemyCounterAttackChance)
                        {
                            int enemyDamage = rng.Next(GameConstants.EnemyMinDamage, GameConstants.EnemyMaxDamage + 1);
                            player.Health -= enemyDamage;

                            if (player.Health <= 0)
                            {
                                player.Health = 0;
                                player.ScreenType = ScreenTypes.Lose;

                                if (player.PlayerId.ToString() != GameConstants.ProtectedPlayerId)
                                {
                                    var itemsToRemove = player.InventoryItems.Where(ii => !ii.IsInBank).ToList();
                                    if (itemsToRemove.Any()) _context.InventoryItems.RemoveRange(itemsToRemove);
                                }

                                await _context.SaveChangesAsync();
                                await transaction.CommitAsync();
                                return new OkObjectResult(new { message = "You died." });
                            }
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new OkObjectResult(new { enemyHealth = enemy.Health, playerHealth = player.Health, itemBroken = activeInstance?.Durability <= 0 });
                    }

                    if (enemy.EnemyType == EnemyType.Dragon)
                    {
                        player.Money += GameConstants.DragonReward;
                        var floor = await _context.Floors.Include(f => f.Building).FirstOrDefaultAsync(f => f.FloorId == player.FloorId);
                        if (floor?.Building != null) floor.Building.IsBossDefeated = true;
                    }

                    if (enemy.ItemInstanceId.HasValue)
                    {
                        floorItem.FloorItemType = FloorItemType.Item;
                        floorItem.ItemInstanceId = enemy.ItemInstanceId;
                        floorItem.Enemy = null;
                    }
                    else
                    {
                        _context.FloorItems.Remove(floorItem);
                    }

                    _context.Enemies.Remove(enemy);
                    player.ScreenType = ScreenTypes.Floor;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new OkObjectResult(new { victory = true, message = "Enemy defeated." });
                }

                var consumableIds = new[] {
                    GameConstants.ItemIdSmallPotion,
                    GameConstants.ItemIdHealthUpgrade,
                    GameConstants.ItemIdCapacityUpgrade
                };

                if (itemData != null && consumableIds.Contains(itemData.ItemId))
                {
                    if (itemData.ItemId == GameConstants.ItemIdSmallPotion)
                    {
                        if (player.Health >= player.MaxHealth)
                            return _errorService.CreateErrorResponse(400, 5004, "Full health.", "Healing Failed");

                        player.Health = Math.Min(player.MaxHealth, player.Health + GameConstants.SmallPotionHealAmount);
                    }
                    else if (itemData.ItemId == GameConstants.ItemIdHealthUpgrade)
                    {
                        player.MaxHealth += GameConstants.HealthUpgradeAmount;
                        player.Health += GameConstants.HealthUpgradeAmount; 
                    }
                    else if (itemData.ItemId == GameConstants.ItemIdCapacityUpgrade)
                    {
                        player.Capacity += GameConstants.CapacityUpgradeAmount;
                    }

                    var activeItem = player.ActiveInventoryItem!;
                    player.ActiveInventoryItemId = null;
                    _context.InventoryItems.Remove(activeItem);
                    _context.ItemInstances.Remove(activeInstance!);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new OkObjectResult(_mapper.Map<PlayerDto>(player));
                }

                string name = itemData?.Name ?? "Fist";
                return _errorService.CreateErrorResponse(400, 5005, $"{name} is not usable this way.", "Invalid Action");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}