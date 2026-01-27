using AutoMapper;
using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        public SaveController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet("{saveString}")]
        public async Task<ActionResult<Guid>> GetPlayerId(string saveString)
        {
            var save = await _context.Saves
                .FirstOrDefaultAsync(s => s.SaveString == saveString);

            if (save == null)
            {
                return NotFound("No player associated with this savestring.");
            }

            return Ok(new { playerId = save.PlayerId });
        }

        [HttpPost("CreateString/{playerId}")]
        public async Task<ActionResult> ClonePlayerRecord(Guid playerId)
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
            clonedPlayer.Floor = null;

            _context.Players.Add(clonedPlayer);

            var originalBuildings = await _context.Buildings
                .Include(b => b.Floors)
                .Where(b => b.PlayerId == playerId)
                .AsNoTracking()
                .ToListAsync();

            foreach (var oldBuilding in originalBuildings)
            {
                var oldBuildingId = oldBuilding.BuildingId;
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
                        var oldFloorId = oldFloor.FloorId;
                        var newFloor = new Floor
                        {
                            BuildingId = newBuilding.BuildingId,
                            Level = oldFloor.Level
                        };

                        _context.Floors.Add(newFloor);
                        await _context.SaveChangesAsync();
                        if (originalPlayer.FloorId == oldFloorId)
                        {
                            clonedPlayer.FloorId = newFloor.FloorId;
                        }
                    }
                }
            }

            foreach (var originalInvItem in originalPlayer.InventoryItems)
            {
                if (originalInvItem.ItemInstance == null) continue;

                var newInstance = new ItemInstance { ItemId = originalInvItem.ItemInstance.ItemId };
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
                SaveString = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            };
            _context.Saves.Add(newSave);

            await _context.SaveChangesAsync();

            return Ok(new { NewSaveString = newSave.SaveString });
        }
    }
}