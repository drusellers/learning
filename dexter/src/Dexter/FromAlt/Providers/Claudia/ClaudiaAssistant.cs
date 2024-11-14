namespace Dexter.FromAlt.Providers.Claudia;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// The Claude version
/// https://docs.anthropic.com/en/api/messages
/// </summary>
public class ClaudiaAssistant : IAssistant
{
    readonly ILogger<ClaudiaAssistant> _logger;
    readonly IOptions<InferenceOptions> _options;

    /// <summary>
    /// ctor
    /// </summary>
    public ClaudiaAssistant(ILogger<ClaudiaAssistant> logger, IOptions<InferenceOptions> options)
    {
        _logger = logger;
        _options = options;
    }


    /// <inheritdoc />
    public IQuestion Ask(string question, Action<IQuestion>? action = null)
    {
        return new ClaudiaQuestion(_options.Value.Anthropic!, _logger, question, action);
    }
}
