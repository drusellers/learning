namespace Dexter.FromAlt;

/// <summary>
/// Encapsulate a question
/// </summary>
public interface IQuestion : IAsyncEnumerable<AssistantMessage>
{
    /// <summary>
    /// Register a tool for user
    /// </summary>
    void RegisterTool(object tool, string methodName, string description);
}
