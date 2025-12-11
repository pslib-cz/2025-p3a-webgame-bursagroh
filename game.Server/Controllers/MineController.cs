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
        /// <returns>A 1D array (List) of BlockDTOs, length 8.</returns>
        [HttpGet("layer/{mineId}/{depth}")]
        public async Task<IActionResult> GetLayerBlocks(int mineId, int depth)
        {
            if (mineId <= 0 || depth < 0)
            {
                return BadRequest("Invalid Mine ID or Depth. Both must be non-negative.");
            }

            try
            {
                // The service returns the list of DTOs
                var blocks = await _mineService.GetOrGenerateLayerBlocksAsync(mineId, depth);

                // Returns 200 OK with the List<BlockDTO>
                return Ok(blocks);
            }
            catch (InvalidOperationException ex)
            {
                // Typically used for business logic errors like "Mine does not exist"
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Catch all other unexpected errors
                return StatusCode(500, "An internal server error occurred: " + ex.Message);
            }
        }

        [HttpGet("test")]
        public IActionResult GetTestEndpoint()
        {
            // Removed parameters since they were unused in the original Get() signature
            return Ok(context.Mines.ToArray());
        }
    }
}