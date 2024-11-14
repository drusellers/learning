namespace Dexter.FromAlt.Providers.OpenAi;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Your smart assistant
/// </summary>
public class OpenAiAssistant : IAssistant
{
    readonly ILogger<OpenAiAssistant> _logger;
    readonly IOptions<InferenceOptions> _options;

    /// <summary>
    /// ctor
    /// </summary>
    public OpenAiAssistant(ILogger<OpenAiAssistant> logger, IOptions<InferenceOptions> options)
    {
        _logger = logger;
        _options = options;
    }

    /// <inheritdoc />
    public IQuestion Ask(string question, Action<IQuestion>? action = null)
    {
        return new OpenAiQuestion(question, _options.Value.OpenAi!, _logger, action);
    }
}
