namespace Dexter.FromAlt;

using Dexter.FromAlt.Providers.Claudia;
using Dexter.FromAlt.Providers.OpenAi;

/// <summary>
/// Inference Options
/// </summary>
public class InferenceOptions
{
    /// <summary>
    /// Config Entry
    /// </summary>
    public const string ConfigKey = "Inference";

    /// <summary>
    /// Open AI
    /// </summary>
    public OpenAiOptions? OpenAi { get; set; }

    /// <summary>
    /// Anthropic
    /// </summary>
    public AnthropicOptions? Anthropic { get; set; }
}
