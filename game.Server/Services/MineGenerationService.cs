using game.Server.Data;
using game.Server.Models;
using Microsoft.EntityFrameworkCore;

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
            int actualEndDepth = endDepth ?? startDepth;
            if (startDepth > actualEndDepth || startDepth < 0) return new List<MineLayer>();

            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existingLayers = await _context.MineLayers
                        .Where(ml => ml.MineId == mineId && ml.Depth >= startDepth && ml.Depth <= actualEndDepth)
                        .Include(ml => ml.MineBlocks)
                            .ThenInclude(mb => mb.Block)
                                .ThenInclude(b => b.Item)
                        .ToListAsync();

                    var existingDepths = existingLayers.Select(l => l.Depth).ToHashSet();
                    var missingDepths = Enumerable.Range(startDepth, actualEndDepth - startDepth + 1)
                                                  .Where(d => !existingDepths.Contains(d))
                                                  .ToList();

                    if (!missingDepths.Any())
                    {
                        return existingLayers.OrderBy(l => l.Depth).ToList();
                    }

                    var mineExists = await _context.Mines.AnyAsync(m => m.MineId == mineId);
                    if (!mineExists) return new List<MineLayer>();

                    var availableBlocks = await _context.Blocks.Include(b => b.Item).ToListAsync();
                    if (!availableBlocks.Any()) return new List<MineLayer>();

                    var totalWeight = availableBlocks.Sum(b => b.Item.ChangeOfGenerating);
                    var random = new Random();

                    foreach (int depth in missingDepths)
                    {
                        var newLayer = new MineLayer
                        {
                            MineId = mineId,
                            Depth = depth,
                            MineBlocks = new List<MineBlock>()
                        };

                        for (int i = 0; i < LayerSize; i++)
                        {
                            var randomNumber = random.Next(totalWeight);
                            var runningWeight = 0;
                            Block blockDefinition = availableBlocks.First();

                            foreach (var block in availableBlocks)
                            {
                                runningWeight += block.Item.ChangeOfGenerating;
                                if (randomNumber < runningWeight)
                                {
                                    blockDefinition = block;
                                    break;
                                }
                            }

                            newLayer.MineBlocks.Add(new MineBlock
                            {
                                MineLayer = newLayer,
                                BlockId = blockDefinition.BlockId,
                                Block = blockDefinition,
                                Index = i
                            });
                        }

                        _context.MineLayers.Add(newLayer);
                        existingLayers.Add(newLayer);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return existingLayers.OrderBy(l => l.Depth).ToList();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
    }
}