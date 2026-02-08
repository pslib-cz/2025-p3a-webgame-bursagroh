using game.Server.Models;

namespace game.Server.Services
{
    public interface IMineGenerationService
    {
        Task<List<MineLayer>> GetOrGenerateLayersBlocksAsync(int mineId, int startDepth, int? endDepth = null);
    }
}