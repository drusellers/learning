namespace Dexter.Tests;

using Dexter.FromAlt.Providers.OpenAi;
using Microsoft.Extensions.DependencyInjection;

public class OpenAiInferenceEngineTests
{
    [Test]
    public async Task Embed()
    {
        using var scope = TestContext.Provider.CreateScope();
        var e = scope.ServiceProvider.GetRequiredService<OpenAiInferenceEngine>();

        var a = await e.GetEmbedding("ABC");
        Assert.That(a.Length, Is.GreaterThan(0));
    }
}
