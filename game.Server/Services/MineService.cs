using game.Server.Data;
using game.Server.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<BlockDTO>> GetOrGenerateLayerBlocksAsync(int mineId, int depth)
        {
            var existingLayer = await _context.MineLayers
                .Where(ml => ml.MineId == mineId && ml.Depth == depth)
                .Include(ml => ml.MineBlocks)
                    .ThenInclude(mb => mb.Block)
                .FirstOrDefaultAsync();

            if (existingLayer != null && existingLayer.MineBlocks.Any())
            {
                return existingLayer.MineBlocks
                    .OrderBy(mb => mb.Index)
                    .Select(mb => new BlockDTO
                    {
                        BlockId = mb.Block.BlockId,
                        BlockType = mb.Block.BlockType.ToString(),
                        ItemId = mb.Block.ItemId,
                        MinAmount = mb.Block.MinAmount,
                        MaxAmount = mb.Block.MaxAmount
                    })
                    .ToList();
            }


            var mineExists = await _context.Mines.AnyAsync(m => m.MineId == mineId);
            if (!mineExists)
            {
                throw new Exception("Lol");
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

            var availableBlocks = await _context.Blocks.ToListAsync();

            if (!availableBlocks.Any())
            {
                throw new Exception("Lol");
            }

            var random = new Random();
            var generatedBlockDTOs = new List<BlockDTO>();

            for (int i = 0; i < LayerSize; i++)
            {
                var blockDefinition = availableBlocks[random.Next(availableBlocks.Count)];

                var mineBlock = new MineBlock
                {
                    MineLayer = layer,
                    BlockId = blockDefinition.BlockId,
                    Index = i
                };
                layer.MineBlocks.Add(mineBlock);
                generatedBlockDTOs.Add(new BlockDTO
                {
                    BlockId = blockDefinition.BlockId,
                    BlockType = blockDefinition.BlockType.ToString(),
                    ItemId = blockDefinition.ItemId,
                    MinAmount = blockDefinition.MinAmount,
                    MaxAmount = blockDefinition.MaxAmount
                });
            }
            await _context.SaveChangesAsync();

            return generatedBlockDTOs;
        }
    }
}