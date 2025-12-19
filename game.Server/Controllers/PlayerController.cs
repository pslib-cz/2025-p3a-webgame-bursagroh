using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

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
                Seed = new Random().Next(),

                PositionX = 0,
                PositionY = 0,
                SubPositionX = 0,
                SubPositionY = 0,
            };

            var fixedBuildings = new List<Building>
            {
                new Building { PlayerId = player.PlayerId, BuildingType = BuildingTypes.Fountain, PositionX = 0, PositionY = 0, IsBossDefeated = false },
                new Building { PlayerId = player.PlayerId, BuildingType = BuildingTypes.Mine, PositionX = 2, PositionY = 0, IsBossDefeated = false },
                new Building { PlayerId = player.PlayerId, BuildingType = BuildingTypes.Bank, PositionX = -2, PositionY = 0, IsBossDefeated = false },
                new Building { PlayerId = player.PlayerId, BuildingType = BuildingTypes.Restaurant, PositionX = 0, PositionY = -2, IsBossDefeated = false },
                new Building { PlayerId = player.PlayerId, BuildingType = BuildingTypes.Blacksmith, PositionX = 0, PositionY = 2, IsBossDefeated = false }
            };

            context.Players.Add(player);
            context.Buildings.AddRange(fixedBuildings);
            await context.SaveChangesAsync();



            context.Players.Update(player);
            await context.SaveChangesAsync();

            return player;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(Guid id) {
            Player? player = await context.Players.Where(p => p.PlayerId == id).FirstOrDefaultAsync(p => p.PlayerId == id);

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

        [HttpPatch("{id}/Action/move-screen")]
        public async Task<ActionResult> MoveScreen(Guid id, [FromBody] MoveScreenRequest newScreenType)
        {
            Player? player = await context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            if (player.ScreenType == ScreenTypes.Floor)
            {
                player.FloorId = null;
            }

            player.ScreenType = newScreenType.NewScreenType;

            await context.SaveChangesAsync();

            return Ok(player);
        }

        [HttpPatch("{id}/Action/move")] //predelat, tohle je fakt hnus, ale funguje to
        public async Task<ActionResult<Player>> MovePlayer(Guid id, [FromBody] MovePlayerRequest request)
        {
            Player? player = await context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null)
            {
                return NotFound();
            }

            if (player.ScreenType == ScreenTypes.City)
            {
                bool isAdjacent = (Math.Abs(request.NewPositionX - player.PositionX) +
                                Math.Abs(request.NewPositionY - player.PositionY)) == 1;

                if (!isAdjacent)
                {
                    return BadRequest("move > 1");
                }

                player.PositionX = request.NewPositionX;
                player.PositionY = request.NewPositionY;
            }
            else if (player.ScreenType == ScreenTypes.Floor)
            {
                if (request.NewFloorId.HasValue && request.NewFloorId != player.FloorId)
                {
                    var floorExists = await context.Floors.AnyAsync(f => f.FloorId == request.NewFloorId);
                    if (!floorExists)
                    {
                        return BadRequest("Target floor does not exist.");
                    }
                    player.FloorId = request.NewFloorId;
                }
                else
                {
                    bool isAdjacent = (Math.Abs(request.NewPositionX - player.SubPositionX) +
                                    Math.Abs(request.NewPositionY - player.SubPositionY)) == 1;

                    if (!isAdjacent)
                    {
                        return BadRequest("move > 1");
                    }

                    player.SubPositionX = request.NewPositionX;
                    player.SubPositionY = request.NewPositionY;
                }
            }
            else
            {
                bool isAdjacent = (Math.Abs((request.NewPositionX - player.SubPositionX)) +
                                Math.Abs(request.NewPositionY - player.SubPositionY)) == 1;

                if (!isAdjacent)
                {
                    return BadRequest("move > 1");
                }

                player.SubPositionX = request.NewPositionX;
                player.SubPositionY = request.NewPositionY;
            }

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