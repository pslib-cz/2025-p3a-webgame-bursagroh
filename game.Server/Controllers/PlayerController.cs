using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public PlayerController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost("Generate")]
        public async Task<Player> Generate([FromBody] GeneratePlayerRequest request)
        {
            Player player = new Player
            {
                PlayerId = Guid.NewGuid(),
                Name = request.Name,
                ScreenType = ScreenTypes.City,
                Money = 0,
                BankBalance = 0,
                Capacity = 10,
                Seed = new Random().Next()
            };

            Building building = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.City,
                PositionX = 0,
                PositionY = 0,
                IsBossDefeated = false
            };

            Building fountain = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Fountain,
                PositionX = 0,
                PositionY = 0,
                IsBossDefeated = false
            };

            Building mine = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Mine,
                PositionX = 2,
                PositionY = 0,
                IsBossDefeated = false
            };

            Building bank = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Bank,
                PositionX = -2,
                PositionY = 0,
                IsBossDefeated = false
            };

            Building restaurant = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Restaurant,
                PositionX = 0,
                PositionY = -2,
                IsBossDefeated = false
            };

            Building blacksmith = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Blacksmith,
                PositionX = 0,
                PositionY = 2,
                IsBossDefeated = false
            };

            Building road1 = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Road,
                PositionX = 1,
                PositionY = 0,
                IsBossDefeated = false
            };

            Building road2 = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Road,
                PositionX = -1,
                PositionY = 0,
                IsBossDefeated = false
            };

            Building road3 = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Road,
                PositionX = 0,
                PositionY = 1,
                IsBossDefeated = false
            };

            Building road4 = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Road,
                PositionX = 1,
                PositionY = 1,
                IsBossDefeated = false
            };

            Building road5 = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Road,
                PositionX = -1,
                PositionY = -1,
                IsBossDefeated = false
            };

            Building road6 = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Road,
                PositionX = 1,
                PositionY = -1,
                IsBossDefeated = false
            };

            Building road7 = new Building
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Road,
                PositionX = -1,
                PositionY = 1,
                IsBossDefeated = false
            };

            Building road8 = new Building 
            {
                PlayerId = player.PlayerId,
                BuildingType = BuildingTypes.Road,
                PositionX = 0,
                PositionY = -1,
                IsBossDefeated = false
            };

            context.Players.Add(player);

            context.Buildings.Add(building);
            context.Buildings.Add(fountain);
            context.Buildings.Add(mine);
            context.Buildings.Add(bank);
            context.Buildings.Add(blacksmith);
            context.Buildings.Add(restaurant);

            context.Buildings.Add(road1);
            context.Buildings.Add(road2);
            context.Buildings.Add(road3);
            context.Buildings.Add(road4);
            context.Buildings.Add(road5);
            context.Buildings.Add(road6);
            context.Buildings.Add(road7);
            context.Buildings.Add(road8);

            await context.SaveChangesAsync();

            Floor floor = new Floor
            {
                BuildingId = building.BuildingId, 
                Level = 1
            };

            context.Floors.Add(floor);
            await context.SaveChangesAsync();

            FloorItem floorItem = new FloorItem
            {
                FloorId = floor.FloorId,
                PositionX = 0,
                PositionY = 0,
                FloorItemType = FloorItemType.Player
            };

            context.FloorItems.Add(floorItem);
            await context.SaveChangesAsync();

            player.BuildingId = building.BuildingId;
            player.FloorItemId = floorItem.FloorItemId;

            context.Players.Update(player);
            await context.SaveChangesAsync();

            return player;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(Guid id) {
            Player? player = await context.Players .Include(p => p.FloorItem).FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpGet("ScreenTypes")]
        public ActionResult<IEnumerable<string>> GetScreenTypes()
        {
            string[] screenTypes = Enum.GetNames<ScreenTypes>();
            return Ok(screenTypes);
        }

        [HttpPatch("{id}/Action/move-screen")] //asi hodim do services
        public async Task<ActionResult> MoveScreen(Guid id, [FromBody] MoveScreenRequest newScreenType)
        {
            Player? player = await context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }


            if (newScreenType.NewScreenType == ScreenTypes.Mine)
            {
                Building? mineBuilding = await context.Buildings
                    .Where(b => b.PlayerId == player.PlayerId && b.BuildingType == BuildingTypes.Mine)
                    .FirstOrDefaultAsync();

                if (mineBuilding == null)
                {
                    mineBuilding = new Building
                    {
                        PlayerId = player.PlayerId,
                        BuildingType = BuildingTypes.Mine,
                        PositionX = 2, //jelikoz jeste nemam udelanou generaci mapy tak je vse na 0,0, sorry jako
                        PositionY = 0 //ona se i tak cela tahle generace budov se bude delat na generovani mapy a ne tady
                    };
                    context.Buildings.Add(mineBuilding);
                    await context.SaveChangesAsync();
                }

                Floor? floor = await context.Floors
                    .Where(f => f.BuildingId == mineBuilding.BuildingId && f.Level == 1)
                    .FirstOrDefaultAsync();

                if (floor == null)
                {
                    floor = new Floor { BuildingId = mineBuilding.BuildingId, Level = 1 };
                    context.Floors.Add(floor);
                    await context.SaveChangesAsync();
                }

                FloorItem? playerFloorItem = await context.FloorItems
                    .Where(fi => fi.FloorId == floor.FloorId && fi.FloorItemType == FloorItemType.Player)
                    .FirstOrDefaultAsync();

                if (playerFloorItem == null)
                {
                    playerFloorItem = new FloorItem
                    {
                        FloorId = floor.FloorId,
                        PositionX = 0,
                        PositionY = 0,
                        FloorItemType = FloorItemType.Player
                    };
                    context.FloorItems.Add(playerFloorItem);
                    await context.SaveChangesAsync();
                }

                player.BuildingId = mineBuilding.BuildingId;
                player.FloorItemId = playerFloorItem.FloorItemId; 
                player.ScreenType = ScreenTypes.Mine;

                await context.SaveChangesAsync();
                return Ok(player);
            }

            if (newScreenType.NewScreenType == ScreenTypes.City)
            {
                Building? cityBuilding = await context.Buildings
                    .Where(b => b.PlayerId == player.PlayerId && b.BuildingType == BuildingTypes.City)
                    .FirstOrDefaultAsync();

                Floor? floor = await context.Floors
                    .Where(f => f.BuildingId == cityBuilding.BuildingId && f.Level == 1)
                    .FirstOrDefaultAsync();

                if (floor == null)
                {
                    floor = new Floor { BuildingId = cityBuilding.BuildingId, Level = 1 };
                    context.Floors.Add(floor);
                    await context.SaveChangesAsync();
                }
                FloorItem? playerFloorItem = await context.FloorItems
                    .Where(fi => fi.FloorId == floor.FloorId && fi.FloorItemType == FloorItemType.Player)
                    .FirstOrDefaultAsync();

                if (playerFloorItem == null)
                {
                    playerFloorItem = new FloorItem
                    {
                        FloorId = floor.FloorId,
                        PositionX = 0,
                        PositionY = 0,
                        FloorItemType = FloorItemType.Player
                    };
                    context.FloorItems.Add(playerFloorItem);
                    await context.SaveChangesAsync();
                }

                player.BuildingId = cityBuilding.BuildingId; 
                player.FloorItemId = playerFloorItem.FloorItemId;
                player.ScreenType = ScreenTypes.City;

                await context.SaveChangesAsync();
                return Ok(player);
            }

            player.ScreenType = newScreenType.NewScreenType;
            await context.SaveChangesAsync();

            return Ok(player);
        }

        [HttpPatch("{id}/Action/move")]
        public async Task<ActionResult<Player>> MovePlayer(Guid id, [FromBody] MovePlayerRequest request)
        {
            Player? player = await context.Players
                .Include(p => p.FloorItem)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null)
            {
                return NotFound();
            }

            FloorItem? currentPositionItem = player.FloorItem;

            if (currentPositionItem == null)
            {
                if (player.FloorItemId.HasValue)
                {
                    currentPositionItem = await context.FloorItems.Where(fi => fi.FloorItemId == player.FloorItemId).FirstOrDefaultAsync();
                }

                if (currentPositionItem == null)
                {
                    return BadRequest();
                }
            }

            if (currentPositionItem.PositionX == request.NewPositionX && currentPositionItem.PositionY == request.NewPositionY)
            {
                return Ok(player);
            }

            bool isAdjacent = (Math.Abs(request.NewPositionX - currentPositionItem.PositionX) +
                               Math.Abs(request.NewPositionY - currentPositionItem.PositionY)) == 1;

            if (!isAdjacent)
            {
                return BadRequest("move > 1: Only adjacent moves are allowed.");
            }

            // 4. Aktualizace pozice
            currentPositionItem.PositionX = request.NewPositionX;
            currentPositionItem.PositionY = request.NewPositionY;

            await context.SaveChangesAsync();
            return Ok(player);
        }

        [HttpGet("{id}/Inventory")]
        public ActionResult<InventoryItem> GetPlayerInventory(Guid id)
        {
            IQueryable<InventoryItem> inventoryItems = context.InventoryItems.Where(i => i.PlayerId == id).Where(i => i.IsInBank == false);

            if (inventoryItems == null)
            {
                return NoContent();
            }

            List<InventoryItem> items = inventoryItems.ToList();
            return Ok(items);
        }

    }
}