namespace CoreLifecycle;

public class RunHostedService : IHostedService
{
    readonly ILogger<RunHostedService> _logger;

    public RunHostedService(ILogger<RunHostedService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("START");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("STOP");

        return Task.CompletedTask;
    }
}
