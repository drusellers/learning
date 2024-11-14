namespace Dexter.FromAlt;

using Dexter.FromAlt.Providers.Claudia;
using Dexter.FromAlt.Providers.OpenAi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Loads Inference Engine Items
/// </summary>
public static class InferenceLoader
{
    /// <summary>
    /// add inference
    /// </summary>
    public static void AddInference(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: InferenceDbContext

        services.AddOptions<InferenceOptions>()
            .Configure(o => configuration.GetSection(InferenceOptions.ConfigKey).Bind(o));

        services.AddScoped<ClaudiaAssistant>();
        services.AddScoped<OpenAiAssistant>();
        services.AddScoped<OpenAiInferenceEngine>();
        services.AddScoped<IInferenceEngine, OpenAiInferenceEngine>();
        // services.AddScoped<IInferenceEngine, ClaudInferenceEngine>();

    }
}
