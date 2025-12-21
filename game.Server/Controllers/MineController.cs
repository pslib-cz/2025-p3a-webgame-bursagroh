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

        [HttpGet("Blocks")]
        public async Task<ActionResult<List<Block>>> GetAllBlocks()
        {
            List<Block> blocks = await context.Blocks.Include(b => b.Item).ToListAsync();

            if (blocks == null || blocks.Count == 0)
            {
                return NotFound();
            }

            return Ok(blocks);
        }

        [HttpGet("Generate")]
        public async Task<IActionResult> GetMine()
        {
            Mine mine = new Mine
            {
                MineId = new Random().Next()
            };

            context.Mines.Add(mine);
            await context.SaveChangesAsync();

            return Ok(mine);
        }

        [HttpGet("{mineId}/Layer/{layer}")]
        public async Task<ActionResult<List<MineBlock>>> GetLayerBlocks(int mineId, int layer) 
        {
            if (mineId <= 0 || layer < 0)
            {
                return BadRequest();
            }

            try
            {
                var blocks = await _mineService.GetOrGenerateLayersBlocksAsync(mineId, layer);
                return Ok(blocks);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{mineId}/Layers")]
        public async Task<ActionResult<List<MineBlock>>> GetLayerBlocksRange(int mineId, [FromQuery] int startLayer, [FromQuery] int endLayer)
        {
            if (mineId <= 0 || startLayer < 0 || endLayer < 0 || startLayer > endLayer)
            {
                return BadRequest("invalid args");
            }

            const int MaxLayerRange = 20;
            if (endLayer - startLayer >= MaxLayerRange)
            {
                return BadRequest($"layers are over {MaxLayerRange}.");
            }

            try
            {
                List<MineBlock> blocks = await _mineService.GetOrGenerateLayersBlocksAsync(mineId, startLayer, endLayer);
                return Ok(blocks);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}