namespace CoreLifecycle;

public class RunHostedLifecycleService : IHostedLifecycleService
{
    readonly ILogger<RunHostedLifecycleService> _logger;

    public RunHostedLifecycleService(ILogger<RunHostedLifecycleService> logger)
    {
        _logger = logger;
    }

    public Task StartingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("STARTING (1)");

        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("START (2)");

        return Task.CompletedTask;
    }

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("STARTED (3)");

        return Task.CompletedTask;
    }


    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("STOPPING (1)");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("STOP (2)");

        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("STOPPED (3)");

        return Task.CompletedTask;
    }


}
