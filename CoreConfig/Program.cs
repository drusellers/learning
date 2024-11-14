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

sc.AddOptions<NamedOptions>()
    .Configure(o =>
    {
        config.GetSection("Bob").Bind(o);
    });
sc.AddOptions<NamedOptions>("Name1")
    .Configure(o =>
    {
        config.GetSection("Media").Bind(o);
    });
sc.AddOptions<NamedOptions>("Name2")
    .Configure(o =>
    {
        config.GetSection("Blah").Bind(o);
    });

var x = sc.BuildServiceProvider(true);

var o = x.GetRequiredService<IOptions<SomeOptions>>();

Console.WriteLine(o.Value);


// Named Configuration Stuff

// We didn't specify a name so we get nothing
var defaultOption = x.GetRequiredService<IOptions<NamedOptions>>();
Console.WriteLine(defaultOption.Value);

// can be called from the root
var monitor = x.GetRequiredService<IOptionsMonitor<NamedOptions>>();
var m = monitor.Get("");
var m1 = monitor.Get("Name1");
var m2 = monitor.Get("Name2");
Console.WriteLine("m:{0}", m);
Console.WriteLine(m1);
Console.WriteLine(m2);

// What about a miss? (you get an unconfigured value .. not the default value)
var m3 = monitor.Get("HUH");
Console.WriteLine("huh: {0}", m3);


// IOptionsSnapshot must be inside scope
using var scope = x.CreateScope();
var snapshot = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<NamedOptions>>();
Console.WriteLine(snapshot.Value);
Console.WriteLine(snapshot.Get("Name1"));
Console.WriteLine(snapshot.Get("Name2"));




Console.WriteLine();
Console.WriteLine("..Change..");
config.GetSection(SomeOptions.ConfigKey)["Name"] = "!UPDATED!";
using var changed = x.CreateScope();
var plain = changed.ServiceProvider.GetRequiredService<IOptions<SomeOptions>>();
Console.WriteLine(plain.Value.Name);
var snap = changed.ServiceProvider.GetRequiredService<IOptionsSnapshot<SomeOptions>>();
Console.WriteLine(snap.Value.Name);
var mon = changed.ServiceProvider.GetRequiredService<IOptionsMonitor<SomeOptions>>();
Console.WriteLine(mon.CurrentValue.Name);






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


public class NamedOptions
{
    public string Name { get; set; }

    public override string ToString()
    {
        return $"Name={Name}";
    }
}
