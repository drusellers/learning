namespace CoreLogging;

using Microsoft.Extensions.Logging;

public static class Scopes
{
    static readonly Func<ILogger, string, IDisposable?> OrganizationScope =
        LoggerMessage.DefineScope<string>(
            "Organization: {OrganizationId}");

    public static IDisposable? BeginOrganizationScope(this ILogger logger, string name)
    {
        return OrganizationScope(logger, name);
    }

}
