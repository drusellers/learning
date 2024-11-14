namespace Dexter.FromAlt;

/// <summary>
/// Assistant Message
/// </summary>
public class AssistantMessage
{
    /// <summary>
    /// ctor
    /// </summary>
    public AssistantMessage(MessageRole role, string message)
    {
        Id = Guid.NewGuid();
        Role = role;
        Message = message;
    }

    /// <summary>
    /// Make a Vogen Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Make my own role
    /// </summary>
    public MessageRole Role { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public string Message { get; set; }
}
