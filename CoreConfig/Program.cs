using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder()
    .AddYamlFile("appsettings.yaml")
    .AddYamlFile("appsettings.Development.yaml", false)
    .AddEnvironmentVariables()
    .Build();

// Dependency Options stuff
// - Microsoft.Extensions.Options
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-8.0
var sc = new ServiceCollection();
sc.AddOptions<SomeOptions>()
    .Configure(o =>
    {
        // Microsoft.Extensions.Configuration.Binder
        config.GetSection("Abc").Bind(o);
    });

var x = sc.BuildServiceProvider(true);




public record SomeOptions;
