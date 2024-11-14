namespace Dexter.FromAlt;

/// <summary>
/// The abstraction over OpenAI/ChatGPT and Anthropic/Claude
/// </summary>
public interface IAssistant
{
    /// <summary>
    /// Ask a question
    /// </summary>
    IQuestion Ask(string question, Action<IQuestion>? action = null);
}
