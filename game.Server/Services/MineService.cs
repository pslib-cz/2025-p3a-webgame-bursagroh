using game.Server.Data;
using game.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Services
{
    public class MineService
    {
        private readonly ApplicationDbContext _context;
        private const int LayerSize = 20; // Define the size of your 1D block array

        public MineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MineBlock>> GetOrGenerateLayerBlocksAsync(int mineId, int depth)
        {
            // 1. Try to find the existing layer
            var existingLayer = await _context.MineLayers
                .Where(ml => ml.MineId == mineId && ml.Depth == depth)
                .Include(ml => ml.MineBlocks)
                .ThenInclude(mb => mb.Block) // Include the Block definition for the data
                .FirstOrDefaultAsync();

            // 2. If layer and blocks exist, return them immediately
            if (existingLayer != null && existingLayer.MineBlocks.Any())
            {
                // Ensure the blocks are sorted by Index to represent the 1D array
                return existingLayer.MineBlocks.OrderBy(mb => mb.Index).ToList();
            }

            // 3. If layer exists but has no blocks, or if the layer doesn't exist, generate a new layer and blocks

            MineLayer layer;
            if (existingLayer != null)
            {
                // Layer exists, but blocks don't (shouldn't happen, but good check)
                layer = existingLayer;
            }
            else
            {
                // Layer does not exist, create a new one
                layer = new MineLayer
                {
                    MineId = mineId,
                    Depth = depth,
                    MineBlocks = new List<MineBlock>()
                };
                _context.MineLayers.Add(layer);
            }

            // Get all Block definitions for generation (assuming IDs 1 and 2 exist from seed)
            var availableBlocks = await _context.Blocks.ToListAsync();

            if (!availableBlocks.Any())
            {
                throw new InvalidOperationException("No Block definitions found to generate a layer.");
            }

            // Generate the 1D array of blocks
            var random = new Random();
            for (int i = 0; i < LayerSize; i++)
            {
                // Simple generation: pick a Block definition randomly
                var blockDefinition = availableBlocks[random.Next(availableBlocks.Count)];

                var mineBlock = new MineBlock
                {
                    MineLayer = layer, // Link back to the new layer instance
                    BlockId = blockDefinition.BlockId,
                    Index = i // The position in the 1D array
                };
                layer.MineBlocks.Add(mineBlock);
            }

            // Save the newly generated layer and blocks to the database
            await _context.SaveChangesAsync();

            // Return the newly generated blocks
            return layer.MineBlocks.OrderBy(mb => mb.Index).ToList();
        }
    }
}