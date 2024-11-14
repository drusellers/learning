namespace SampleDbJobs;

using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

public class SampleDbContext: DbContext
{
    public SampleDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // MT Jobs
        new JobTypeSagaMap(false).Configure(modelBuilder);
        new JobSagaMap(false).Configure(modelBuilder);
        new JobAttemptSagaMap(false).Configure(modelBuilder);
    }
}
