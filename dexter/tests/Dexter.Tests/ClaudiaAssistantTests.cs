namespace Dexter.Tests;

using Dexter.FromAlt.Providers.Claudia;
using Microsoft.Extensions.DependencyInjection;

public class ClaudiaAssistantTests
{
    [Explicit]
    [Test]
    public async Task Abc()
    {
        using var scope = TestContext.Provider.CreateScope();
        var assistant = scope.ServiceProvider.GetRequiredService<ClaudiaAssistant>();

        var at = new AssetsTool();

        await foreach (var m in assistant.Ask("When does Gusto renew?", o =>
                       {
                           o.RegisterTool(at, "GetAssets", "Get a list of assets, sorted by descending amount");
                           o.RegisterTool(at, "GetAsset", "Get an asset by name");
                       }))
        {
            Console.WriteLine("R:{0} - M:{1}", m.Role, m.Message);
        }
    }
}
