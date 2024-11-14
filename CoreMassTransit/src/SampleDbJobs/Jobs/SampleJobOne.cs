namespace SampleDbJobs.Jobs;

using MassTransit;

public class SampleJobOne: IJobConsumer<SampleJobOne.Execute>
{
    readonly IServiceProvider _provider;

    public SampleJobOne(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task Run(JobContext<Execute> context)
    {
        using var scope = _provider.CreateScope();

        Console.WriteLine("A JOB!");

        return Task.CompletedTask;
    }

    public record Execute;
}
