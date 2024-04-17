namespace CoreLifecycle;

public class RunBackgroundService : BackgroundService
{
    readonly ILogger<RunBackgroundService> _logger;

    public RunBackgroundService(ILogger<RunBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{Name} ExecuteAsync", nameof(RunBackgroundService));
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("{Name} tick: {Ts}", nameof(RunBackgroundService), DateTime.UtcNow);

            await Task.Delay(1000, stoppingToken);
        }

    }
}
