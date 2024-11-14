namespace Dexter.FromAlt;

/// <summary>
/// The role of the message
/// </summary>
public enum MessageRole
{
    /// <summary>
    /// A system message
    /// </summary>
    System,

    /// <summary>
    /// an assistant message
    /// </summary>
    Assistant,

    /// <summary>
    /// a user message
    /// </summary>
    User,

    /// <summary>
    /// a tool message
    /// </summary>
    Tool
}
