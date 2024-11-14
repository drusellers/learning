namespace Dexter.Tests;

using Dexter.FromAlt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[SetUpFixture]
public static class TestContext
{
    static ServiceProvider _provider = null!;

    [OneTimeSetUp]
    public static async Task OneTimeSetUp()
    {
        var configuration =  new ConfigurationBuilder()
            .AddYamlFile("appsettings.yaml", true)
            .AddYamlFile("appsettings.Test.yaml", true)
            .AddEnvironmentVariables()
            .Build();

        var collection = new ServiceCollection();
        collection.AddInference(configuration);
        collection.AddLogging();
        _provider = collection.BuildServiceProvider(true);

        await Task.Yield();
    }

    public static IServiceProvider Provider => _provider;

    public static T GetRequiredService<T>() where T : notnull
    {
        return _provider.GetRequiredService<T>();
    }

    [OneTimeTearDown]
    public static void OneTimeTearDown()
    {
        _provider.Dispose();
    }
}
