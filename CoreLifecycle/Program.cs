using CoreLifecycle;

Console.WriteLine("1. before host");

var builder = new HostBuilder();
builder.ConfigureLogging(l =>
{
    l.AddConsole();
});
builder.ConfigureServices(services =>
{
    services.AddHostedService<RunBackgroundService>();
    services.AddHostedService<RunHostedService>();
    services.AddHostedService<RunHostedLifecycleService>();
});

Console.WriteLine("2. before build");
var app = builder.Build();
Console.WriteLine("3. after build - before run");

app.Run();
