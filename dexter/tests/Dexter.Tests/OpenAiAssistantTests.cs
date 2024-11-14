namespace Dexter.Tests;

using Dexter.FromAlt.Providers.OpenAi;
using Microsoft.Extensions.DependencyInjection;

public class OpenAiAssistantTests
{
    [Explicit]
    [Test]
    public async Task Abc()
    {
        using var scope = TestContext.Provider.CreateScope();
        var assistant = scope.ServiceProvider.GetRequiredService<OpenAiAssistant>();
        var at = new AssetsTool();

        await foreach (var m in assistant.Ask("What is the most expensive asset?", o =>
                       {
                           o.RegisterTool(at, "GetAssets", "Get a list of assets, sorted by descending amount");
                       }))
        {
            Console.WriteLine("R:{0} - M:{1}", m.Role, m.Message);
        }
    }

    [Explicit]
    [Test]
    public async Task When_does_it_renew()
    {
        using var scope = TestContext.Provider.CreateScope();
        var assistant = scope.ServiceProvider.GetRequiredService<OpenAiAssistant>();
        var at = new AssetsTool();

        await foreach (var m in assistant.Ask("When does elastic renew?", o =>
                       {
                           o.RegisterTool(at, "GetAssets", "Get a list of assets, sorted by descending amount");
                           o.RegisterTool(at, "GetAsset", "Get an asset by name");
                       }))
        {
            Console.WriteLine("R:{0} - M:{1}", m.Role, m.Message);
        }
    }
}
