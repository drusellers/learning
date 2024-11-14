using MassTransit;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using SampleDbJobs;
using SampleDbJobs.Jobs;
using Serilog;
using Serilog.Events;

var format = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
// var format = "[{Timestamp:HH:mm:ss} {SourceContext} {Level:u3}] {Message:lj}{NewLine}{Exception}";

// This creates a global logger that can be used during
// bootstrap, but can later be configured using the host
// builder
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: format)
    .CreateBootstrapLogger();

// Attach a global error handler
AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
{
    Log.Fatal((Exception)eventArgs.ExceptionObject, "Global Unhandled Exception");
};

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.WriteTo.Console();
    configuration.MinimumLevel.Override("Microsoft", LogEventLevel.Error);
});
// default configuration


var conn = "Host=localhost;Port=5432;Database=jobs;Username=learning;Password=learning;Include Error Detail=true;";
builder.Services.AddDbContext<SampleDbContext>(b =>
    b.UseNpgsql(conn, m =>
    {
        m.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
        m.MigrationsHistoryTable($"__{nameof(SampleDbContext)}");
    }));

builder.Services.AddQuartz(q =>
{
    var maxConcurrency = Environment.ProcessorCount * 5;
    q.MisfireThreshold = TimeSpan.FromSeconds(60);
    q.UseDefaultThreadPool(options => { options.MaxConcurrency = maxConcurrency; });
    q.SetProperty(StdSchedulerFactory.PropertySchedulerMaxBatchSize, maxConcurrency.ToString());

    q.SchedulerId = "AUTO";

    q.UsePersistentStore(s =>
    {
        s.UseProperties = true;
        s.UseClustering();
        s.UsePostgres(conn);
        s.UseNewtonsoftJsonSerializer();
    });
});
builder.Services.AddQuartzHostedService(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = false;
    options.StartDelay = TimeSpan.FromMinutes(2);
});

builder.Services.AddOptions<SqlTransportOptions>()
    .Configure(options =>
    {
        options.ConnectionString = conn;
    });

builder.Services.AddPostgresMigrationHostedService(create: true, delete: false);

builder.Services.AddMassTransit(cfg =>
{
    // needed so we can pull it from the container
    cfg.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(false));

    cfg.AddConsumers(typeof(Program).Assembly);
    cfg.AddSqlMessageScheduler();

    cfg.SetJobConsumerOptions();

    cfg.AddSagaRepository<JobSaga>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<SampleDbContext>();
            r.UsePostgres();
        });

    cfg.AddSagaRepository<JobTypeSaga>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<SampleDbContext>();
            r.UsePostgres();
        });

    cfg.AddSagaRepository<JobAttemptSaga>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<SampleDbContext>();
            r.UsePostgres();
        });

    cfg.AddJobSagaStateMachines(options =>
        {
            options.FinalizeCompleted = true;
        })
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<SampleDbContext>();
            r.UsePostgres();
        });

    cfg.UsingPostgres((context, transport) =>
    {
        transport.UseSqlMessageScheduler();
        transport.UseJobSagaPartitionKeyFormatters();
        transport.ConfigureEndpoints(context);
    });
});


builder.WebHost.UseKestrel(k =>
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
    k.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

app.MapPost("/", async context =>
{
    await context.RequestServices.GetRequiredService<IPublishEndpoint>()
        .SubmitJob<SampleJobOne.Execute>(new SampleJobOne.Execute());

    context.Response.StatusCode = 200;
});
app.Run();

public static partial class Program;
