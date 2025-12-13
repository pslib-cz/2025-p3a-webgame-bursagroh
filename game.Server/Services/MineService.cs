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
                .Include(ml => ml.MineBlocks).ThenInclude(mb => mb.Block).ThenInclude(b => b.Item) // Include Item for ChangeOfGenerating
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
                // Better error message is helpful!
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

            // --- Updated Block Retrieval to include Item ---
            var availableBlocks = await _context.Blocks
                .Include(b => b.Item) // Ensure Item and its ChangeOfGenerating is available
                .ToListAsync();

            if (!availableBlocks.Any())
            {
                throw new InvalidOperationException("No block definitions available to generate mine layer.");
            }

            // --- Weighted Random Selection Logic ---
            var totalWeight = availableBlocks.Sum(b => b.Item.ChangeOfGenerating);

            if (totalWeight <= 0)
            {
                // Handle case where all weights are 0 or less
                throw new InvalidOperationException("Total weight for block generation is zero or less. Check 'ChangeOfGenerating' values.");
            }


            var random = new Random();
            var generatedMineBlocks = new List<MineBlock>();

            for (int i = 0; i < LayerSize; i++)
            {
                // 1. Pick a random number between 0 and totalWeight - 1
                var randomNumber = random.Next(totalWeight);

                Block blockDefinition = null!;
                var runningWeight = 0;

                // 2. Iterate through blocks, adding up the weights until runningWeight exceeds randomNumber
                foreach (var block in availableBlocks)
                {
                    runningWeight += block.Item.ChangeOfGenerating;
                    if (randomNumber < runningWeight)
                    {
                        blockDefinition = block;
                        break;
                    }
                }

                // Should not happen if logic is correct and totalWeight > 0
                if (blockDefinition == null)
                {
                    // Fallback to a default or first block if selection somehow fails
                    blockDefinition = availableBlocks.First();
                }

                // Use the selected blockDefinition to create the MineBlock
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