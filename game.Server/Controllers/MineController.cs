using game.Server.Data;
using game.Server.Services;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Gets an existing MineLayer's blocks or generates a new one if it doesn't exist.
        /// </summary>
        /// <param name="mineId">The ID of the Mine instance.</param>
        /// <param name="depth">The depth of the layer to retrieve/generate.</param>
        /// <returns>A 1D array (List) of MineBlock instances.</returns>
        [HttpGet("layer/{mineId}/{depth}")]
        public async Task<IActionResult> GetLayerBlocks(int mineId, int depth)
        {
            if (mineId <= 0 || depth < 0)
            {
                return BadRequest("Invalid Mine ID or Depth.");
            }

            try
            {
                var blocks = await _mineService.GetOrGenerateLayerBlocksAsync(mineId, depth);

                // You might want to return a DTO instead of the full entity,
                // but for simplicity, we return the generated list.
                return Ok(blocks);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpGet("test")]
        public async Task<IActionResult> Get(int mineId, int depth)
        {
            return Ok(context.Mines.ToArray());
        }
    }
}