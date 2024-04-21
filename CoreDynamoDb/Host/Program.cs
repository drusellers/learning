using Id.Host.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(cfg =>
    {
        cfg.AddYamlFile("appsettings.yaml");
    })
    .ConfigureServices(services =>
    {
        services.AddOptions<DynamoOptions>()
            .Configure((DynamoOptions o, IConfiguration cfg) =>
            {
                cfg.GetSection(DynamoOptions.ConfigKey).Bind(o);
            });

        services.AddTransient<DynamoContext>();
        services.AddTransient<IDynamoContext, DynamoContext>();
        services.AddHostedService<RootUserLoader>();
    }).Build();

await host.RunAsync();
