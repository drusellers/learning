using CoreConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// Environment Name is case insensitive
Environment.SetEnvironmentVariable("Abc__env","WASA");

var config = new ConfigurationBuilder()
    .AddYamlFile("appsettings.yaml")
    .AddYamlFile("appsettings.Development.yaml", false)
    // .AddDotEnvFile()
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
        config.GetSection(SomeOptions.ConfigKey).Bind(o);
    });

var x = sc.BuildServiceProvider(true);

var o = x.GetRequiredService<IOptions<SomeOptions>>();

Console.WriteLine(o.Value);


public class SomeOptions
{
    public const string ConfigKey = "Abc";

    public string Name { get; set; }
    public int Age { get; set; }
    public string Env { get; set; }

    public override string ToString()
    {
        return $"Name={Name} Age={Age} Env={Env}";
    }
}
