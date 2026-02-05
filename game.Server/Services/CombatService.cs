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
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) return new NotFoundObjectResult("Player not found.");
            if (player.ActiveInventoryItem?.ItemInstance?.Item == null) return new BadRequestObjectResult("Holding nothing.");

            var activeItem = player.ActiveInventoryItem;
            var itemData = activeItem.ItemInstance.Item;

            if (itemData.ItemType == ItemTypes.Sword)
            {
                if (player.ScreenType != ScreenTypes.Fight) return new BadRequestObjectResult("Only weapons in fight.");

                var floorItem = await _context.FloorItems.Include(fi => fi.Enemy)
                    .FirstOrDefaultAsync(fi => fi.FloorId == player.FloorId && fi.PositionX == player.SubPositionX && fi.PositionY == player.SubPositionY);

                if (floorItem?.Enemy == null)
                {
                    player.ScreenType = ScreenTypes.Floor;
                    await _context.SaveChangesAsync();
                    return new BadRequestObjectResult("No enemy found.");
                }

                var enemy = floorItem.Enemy;
                enemy.Health -= itemData.Damage;

                if (enemy.Health > 0)
                {
                    await _context.SaveChangesAsync();
                    return new OkObjectResult(new { message = $"Hit {enemy.EnemyType}!", enemyHealth = enemy.Health });
                }

                if (enemy.ItemInstanceId.HasValue) { floorItem.FloorItemType = FloorItemType.Item; floorItem.ItemInstanceId = enemy.ItemInstanceId; floorItem.Enemy = null; }
                else _context.FloorItems.Remove(floorItem);

                _context.Enemies.Remove(enemy);
                player.ScreenType = ScreenTypes.Floor;
                await _context.SaveChangesAsync();
                return new OkObjectResult(new { victory = true });
            }
            else if (new[] { 40, 41, 42 }.Contains(itemData.ItemId))
            {
                if (itemData.ItemId == 40)
                {
                    if (player.Health >= player.MaxHealth) return new BadRequestObjectResult("Full health.");
                    player.Health = Math.Min(player.MaxHealth, player.Health + 5);
                }
                else if (itemData.ItemId == 41) player.MaxHealth += 5;
                else if (itemData.ItemId == 42) player.Capacity += 5;

                var instance = activeItem.ItemInstance;
                player.ActiveInventoryItemId = null;
                _context.InventoryItems.Remove(activeItem);
                _context.ItemInstances.Remove(instance);
                await _context.SaveChangesAsync();

                return new OkObjectResult(_mapper.Map<PlayerDto>(player));
            }

            return new BadRequestObjectResult($"{itemData.Name} not usable.");
        }
    }
}
