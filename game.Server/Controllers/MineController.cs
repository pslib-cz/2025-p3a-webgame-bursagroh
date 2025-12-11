using game.Server.Data;
using game.Server.Models;
using game.Server.Services;
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

        [HttpGet("layer/{mineId}/{depth}")]
        public async Task<IActionResult> GetLayerBlocks(int mineId, int depth)
        {
            if (mineId <= 0 || depth < 0)
            {
                return BadRequest();
            }

            try
            {
                var blocks = await _mineService.GetOrGenerateLayerBlocksAsync(mineId, depth);
                return Ok(blocks);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred: " + ex.Message);
            }
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetMine(Guid playerId)
        {
            if (playerId == Guid.Empty)
            {
                return BadRequest();
            }

            Mine mine = new Mine
            {
                PlayerId = playerId,
                MineId = new Random().Next()
            };

            context.Mines.Add(mine);

            return Ok(mine);
        }

        [HttpGet("test")]
        public IActionResult GetTestEndpoint()
        {
            return Ok(context.Mines.ToArray());
        }
    }
}