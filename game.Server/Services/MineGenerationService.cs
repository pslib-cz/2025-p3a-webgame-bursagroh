using game.Server.Data;
using game.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace game.Server.Services
{
    public class MineGenerationService
    {
        private readonly ApplicationDbContext _context;
        private const int LayerSize = 8;

        public MineGenerationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<MineLayer>> GetOrGenerateLayersBlocksAsync(int mineId, int startDepth, int? endDepth = null)
        {
            int actualEndDepth = endDepth.HasValue ? endDepth.Value : startDepth;

            if (startDepth > actualEndDepth)
            {
                throw new ArgumentException("Start depth cannot be greater than end depth.");
            }
            if (startDepth < 0)
            {
                throw new ArgumentException("Depth cannot be negative.");
            }

            var mineExists = await _context.Mines.AnyAsync(m => m.MineId == mineId);
            if (!mineExists)
            {
                throw new InvalidOperationException($"Mine with ID {mineId} does not exist.");
            }

            var availableBlocks = await _context.Blocks.Include(b => b.Item).ToListAsync();
            if (!availableBlocks.Any())
            {
                throw new InvalidOperationException("No block definitions available to generate mine layer.");
            }

            var existingLayers = await _context.MineLayers
                .Where(ml => ml.MineId == mineId && ml.Depth >= startDepth && ml.Depth <= actualEndDepth)
                .Include(ml => ml.MineBlocks)
                    .ThenInclude(mb => mb.Block)
                        .ThenInclude(b => b.Item)
                .ToListAsync();

            var totalWeight = availableBlocks.Sum(b => b.Item.ChangeOfGenerating);
            var random = new Random();
            bool changesMade = false;

            for (int depth = startDepth; depth <= actualEndDepth; depth++)
            {
                var currentLayer = existingLayers.FirstOrDefault(ml => ml.Depth == depth);

                if (currentLayer == null)
                {
                    currentLayer = new MineLayer
                    {
                        MineId = mineId,
                        Depth = depth,
                        MineBlocks = new List<MineBlock>()
                    };
                    _context.MineLayers.Add(currentLayer);
                    existingLayers.Add(currentLayer);
                    changesMade = true;
                }

                if (!currentLayer.MineBlocks.Any())
                {
                    for (int i = 0; i < LayerSize; i++)
                    {
                        var randomNumber = random.Next(totalWeight);
                        Block blockDefinition = availableBlocks.First();
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

                        var mineBlock = new MineBlock
                        {
                            MineLayer = currentLayer,
                            BlockId = blockDefinition.BlockId,
                            Block = blockDefinition,
                            Index = i
                        };
                        currentLayer.MineBlocks.Add(mineBlock);
                    }
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                await _context.SaveChangesAsync();
            }

            return existingLayers.OrderBy(l => l.Depth).ToList();
        }
    }
}