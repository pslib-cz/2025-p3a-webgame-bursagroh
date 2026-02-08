using game.Server.Data;
using Microsoft.EntityFrameworkCore;

public class PlayerCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PlayerCleanupService> _logger;

    public PlayerCleanupService(IServiceProvider serviceProvider, ILogger<PlayerCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cleanup service starting: Performing initial purge...");
        await PurgeInactivePlayers();

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRunTime = now.Date.AddHours(3);

            if (now >= nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1);
            }

            var delay = nextRunTime - now;

            _logger.LogInformation($"Next scheduled purge at: {nextRunTime:yyyy-MM-dd HH:mm:ss}. Sleeping for {delay.TotalHours:F2} hours.");

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }

            await PurgeInactivePlayers();
        }
    }

    private async Task PurgeInactivePlayers()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var saveService = scope.ServiceProvider.GetRequiredService<ISaveService>();

        _logger.LogInformation("Starting daily inactive player purge...");

        var cutoffDate = DateTime.UtcNow.AddMonths(-3);
        var protectedPlayerId = Guid.Parse("4b1e8a93-7d92-4f7f-80c1-525c345b85e0");

        var inactivePlayerIds = await context.Players
            .Where(p => p.LastModified < cutoffDate && p.PlayerId != protectedPlayerId)
            .Select(p => p.PlayerId)
            .ToListAsync();

        _logger.LogInformation($"Found {inactivePlayerIds.Count} inactive players to delete.");

        foreach (var playerId in inactivePlayerIds)
        {
            try
            {
                await saveService.DeletePlayerDataAsync(playerId);
                _logger.LogInformation($"Successfully purged player: {playerId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to purge player {playerId}: {ex.Message}");
            }
        }
    }
}