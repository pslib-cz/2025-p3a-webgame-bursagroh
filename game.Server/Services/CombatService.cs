using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Types;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class CombatService : ICombatService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CombatService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResult> UseItemAsync(Guid id)
        {
            var player = await _context.Players
                .Include(p => p.ActiveInventoryItem).ThenInclude(ai => ai.ItemInstance).ThenInclude(ii => ii.Item)
                .Include(p => p.InventoryItems)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) return new NotFoundObjectResult("Player not found.");

            int playerDamage = 1;
            string itemName = "Fist";
            var itemType = ItemTypes.Sword;

            if (player.ActiveInventoryItem?.ItemInstance?.Item != null)
            {
                playerDamage = player.ActiveInventoryItem.ItemInstance.Item.Damage;
                itemName = player.ActiveInventoryItem.ItemInstance.Item.Name;
                itemType = player.ActiveInventoryItem.ItemInstance.Item.ItemType;
            }

            if (itemType == ItemTypes.Sword)
            {
                if (player.ScreenType != ScreenTypes.Fight)
                    return new BadRequestObjectResult("You can only attack during a fight.");

                var floorItem = await _context.FloorItems
                    .Include(fi => fi.Enemy)
                    .FirstOrDefaultAsync(fi => fi.FloorId == player.FloorId &&
                                               fi.PositionX == player.SubPositionX &&
                                               fi.PositionY == player.SubPositionY);

                if (floorItem?.Enemy == null)
                {
                    player.ScreenType = ScreenTypes.Floor;
                    await _context.SaveChangesAsync();
                    return new BadRequestObjectResult("No enemy found here.");
                }

                var enemy = floorItem.Enemy;
                var rng = new Random();

                enemy.Health -= playerDamage;

                if (enemy.Health > 0)
                {
                    if (rng.NextDouble() < 0.07)
                    {
                        int enemyDamage = rng.Next(1, 3);
                        player.Health -= enemyDamage;

                        if (player.Health <= 0)
                        {
                            player.Health = 0;
                            player.ScreenType = ScreenTypes.Lose;
                            var itemsToRemove = player.InventoryItems.Where(ii => !ii.IsInBank).ToList();
                            if (itemsToRemove.Any()) _context.InventoryItems.RemoveRange(itemsToRemove);

                            await _context.SaveChangesAsync();
                            return new OkObjectResult(new { message = "You died." });
                        }
                    }

                    await _context.SaveChangesAsync();
                    return new OkObjectResult(new
                    {
                        enemyHealth = enemy.Health,
                        playerHealth = player.Health
                    });
                }

                if (enemy.EnemyType == EnemyType.Dragon)
                {
                    player.Money += 750;

                    var floor = await _context.Floors
                        .Include(f => f.Building)
                        .FirstOrDefaultAsync(f => f.FloorId == player.FloorId);

                    if (floor?.Building != null)
                    {
                        floor.Building.IsBossDefeated = true;
                    }
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
                return new OkObjectResult(new { victory = true, message = "Enemy defeated." });
            }

            if (player.ActiveInventoryItem != null)
            {
                var itemData = player.ActiveInventoryItem.ItemInstance.Item;
                if (new[] { 40, 41, 42 }.Contains(itemData.ItemId))
                {
                    if (itemData.ItemId == 40)
                    {
                        if (player.Health >= player.MaxHealth) return new BadRequestObjectResult("Full health.");
                        player.Health = Math.Min(player.MaxHealth, player.Health + 5);
                    }
                    else if (itemData.ItemId == 41) player.MaxHealth += 5;
                    else if (itemData.ItemId == 42) player.Capacity += 5;

                    var activeItem = player.ActiveInventoryItem;
                    var instance = activeItem.ItemInstance;

                    player.ActiveInventoryItemId = null;
                    _context.InventoryItems.Remove(activeItem);
                    _context.ItemInstances.Remove(instance);
                    await _context.SaveChangesAsync();

                    return new OkObjectResult(_mapper.Map<PlayerDto>(player));
                }
            }

            return new BadRequestObjectResult($"{itemName} is not usable this way.");
        }
    }
}