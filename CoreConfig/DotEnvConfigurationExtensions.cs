namespace CoreConfig;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

public static class DotEnvConfigurationExtensions
{
    public static IConfigurationBuilder AddDotEnvFile(this IConfigurationBuilder builder)
    {
        builder.Add(new DotEnvConfigurationSource());
        return builder;
    }
}

public record DotEnvOptions(string FileName);
public class DotEnvConfigurationSource: IConfigurationSource
{
    readonly DotEnvOptions _options = new DotEnvOptions(".env");

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        // Validate(_options)

        return new DotEnvConfigurationProvider(_options);
    }
}

public class DotEnvConfigurationProvider : IConfigurationProvider
{
    readonly DotEnvOptions _options;

    public DotEnvConfigurationProvider(DotEnvOptions options)
    {
        _options = options;
    }

    public bool TryGet(string key, out string? value)
    {
        throw new NotImplementedException();
    }

    public void Set(string key, string? value)
    {
        throw new NotImplementedException();
    }

    public IChangeToken GetReloadToken()
    {
        throw new NotImplementedException();
    }

    public void Load()
    {
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, _options.FileName);

        throw new NotImplementedException();
    }

    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        throw new NotImplementedException();
    }
}
