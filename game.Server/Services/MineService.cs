using game.Server.Data;
using game.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace game.Server.Services
{
    public class MineService
    {
        private readonly ApplicationDbContext _context;
        private const int LayerSize = 8;

        public MineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MineBlock>> GetOrGenerateLayerBlocksAsync(int mineId, int depth)
        {
            var existingLayer = await _context.MineLayers
                .Where(ml => ml.MineId == mineId && ml.Depth == depth)
                .Include(ml => ml.MineBlocks).ThenInclude(mb => mb.Block).ThenInclude(b => b.Item) 
                .FirstOrDefaultAsync();

            if (existingLayer != null && existingLayer.MineBlocks.Any())
            {
                return existingLayer.MineBlocks
                    .OrderBy(mb => mb.Index)
                    .ToList();
            }

            var mineExists = await _context.Mines.AnyAsync(m => m.MineId == mineId);
            if (!mineExists)
            {
                throw new InvalidOperationException($"Mine with ID {mineId} does not exist.");
            }

            MineLayer layer;
            if (existingLayer != null)
            {
                layer = existingLayer;
            }
            else
            {
                layer = new MineLayer
                {
                    MineId = mineId,
                    Depth = depth,
                    MineBlocks = new List<MineBlock>()
                };
                _context.MineLayers.Add(layer);
            }

            var availableBlocks = await _context.Blocks
                .Include(b => b.Item) 
                .ToListAsync();

            if (!availableBlocks.Any())
            {
                throw new InvalidOperationException("No block definitions available to generate mine layer.");
            }

            var totalWeight = availableBlocks.Sum(b => b.Item.ChangeOfGenerating);


            var random = new Random();
            var generatedMineBlocks = new List<MineBlock>();

            for (int i = 0; i < LayerSize; i++)
            {
               
                var randomNumber = random.Next(totalWeight);

                Block blockDefinition = null!;
                var runningWeight = 0;

                
                foreach (var block in availableBlocks)
                {
                    runningWeight += block.Item.ChangeOfGenerating;
                    if (randomNumber < runningWeight)
                    {
                        blockDefinition = block;
                        break;
                    }
                }

                if (blockDefinition == null)
                {
                    blockDefinition = availableBlocks.First();
                }

                var mineBlock = new MineBlock
                {
                    MineLayer = layer,
                    BlockId = blockDefinition.BlockId,
                    Block = blockDefinition,
                    Index = i
                };
                layer.MineBlocks.Add(mineBlock);

                generatedMineBlocks.Add(mineBlock);
            }

            await _context.SaveChangesAsync();
            return generatedMineBlocks;
        }
    }
}