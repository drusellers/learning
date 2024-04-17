namespace CoreLogging;

using Microsoft.Extensions.Logging;

public static partial class LoggingFwk
{
    [LoggerMessage(EventId = 10, Level = LogLevel.Information, Message = "Abc {Def}")]
    public static partial void CheckIt(this ILogger logger, string def);
}
