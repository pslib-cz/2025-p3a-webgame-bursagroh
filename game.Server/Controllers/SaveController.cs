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

        public SaveController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("{saveString}")]
        public async Task<ActionResult<Guid>> GetPlayerId(string saveString)
        {
            var save = await _context.Saves
                .FirstOrDefaultAsync(s => s.SaveString == saveString);

            if (save == null)
            {
                return NotFound("No player associated with this SaveString.");
            }

            return Ok(save.PlayerId);
        }


        [HttpGet("Nigger/{playerId}")]
        public async Task<ActionResult<string>> GetSaveString(Guid playerId)
        {
            var playerExists = await _context.Players.AnyAsync(p => p.PlayerId == playerId);
            if (!playerExists) return NotFound("Player not found.");

            var save = await _context.Saves.FirstOrDefaultAsync(s => s.PlayerId == playerId);

            if (save == null)
            {
                save = new Save
                {
                    PlayerId = playerId,
                    SaveString = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
                };
                _context.Saves.Add(save);
                await _context.SaveChangesAsync();
            }

            return Ok(save.SaveString);
        }
    }
}