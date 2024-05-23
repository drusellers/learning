// See https://aka.ms/new-console-template for more information

using Dexter;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

var levelSwitch = new LoggingLevelSwitch();
using var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.ControlledBy(levelSwitch)
    // .WriteTo.Console(new RenderedCompactJsonFormatter())
    .WriteTo.Console()
    .CreateLogger();
Log.Logger = logger;

var config = new ConfigurationBuilder()
    .AddYamlFile("appsettings.yaml", true)
    .AddYamlFile("appsettings.Development.yaml", true)
    // .AddDotEnvFile()
    // .AddEnvironmentVariables()
    .Build();

var oa = config.GetSection(OpenAiOptions.ConfigKey).Get<OpenAiOptions>();

var driver = new Driver(oa!, "What is the user's name?");

// TODO: Add Events to Trigger Hub Events
// TODO: Enumerate "NEW" events

// draw initial messages
await foreach (var message in driver)
{
    Console.WriteLine($"{message.Role}: {message}");
}

Console.WriteLine("--x--");
Console.WriteLine($"Total: {driver.TotalTokens}");
Console.WriteLine($"Remaining: {driver.RemainingTokens}");


public record OpenAiOptions(string ApiKey)
{
    public static string ConfigKey = "OpenAi";
}
