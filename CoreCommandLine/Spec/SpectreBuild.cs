namespace CoreCommandLine.Spec;

using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

public static class SpectreBuild
{
    public static int Go(string[] args)
    {
        var registrations = new ServiceCollection();

        // Create a type registrar and register any dependencies.
        // A type registrar is an adapter for a DI framework.
        var registrar = new TypeRegistrar(registrations);

        var app = new CommandApp(registrar);
        app.Configure(cfg =>
        {
            cfg.AddBranch("orders", o =>
            {
                o.AddCommand<CreateCommand>("create");
                o.AddCommand<ImportCommand>("import");
            });
        });

        return app.Run(args);
    }
}

public class CreateCommand: Command<CreateArgs>
{
    public ValidationResult Validate(CommandContext context, CommandSettings settings)
    {
        return ValidationResult.Success();
    }

    public override int Execute(CommandContext context, CreateArgs settings)
    {
        Console.WriteLine("YAY");
        return 0;
    }
}

public class CreateArgs : CommandSettings
{
    [CommandArgument(0, "[name]")]
    public string? Name { get; set; }

    [CommandOption("-c|--count")]
    public int? Count { get; set; }
}

public class ImportCommand: Command<ImportArgs>
{
    public ValidationResult Validate(CommandContext context, CommandSettings settings)
    {
        return ValidationResult.Success();
    }

    public override int Execute(CommandContext context, ImportArgs settings)
    {
        AnsiConsole.Markup("YAY - [blue]{0}[/] ", settings.File.Exists);
        return 0;
    }
}

public class ImportArgs : CommandSettings
{
    [CommandArgument(0, "[file]")] public FileInfo File { get; set; } = null!;
}

public sealed class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _builder;

    public TypeRegistrar(IServiceCollection builder)
    {
        _builder = builder;
    }

    public ITypeResolver Build()
    {
        return new TypeResolver(_builder.BuildServiceProvider());
    }

    public void Register(Type service, Type implementation)
    {
        _builder.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        _builder.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> func)
    {
        if (func is null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        _builder.AddSingleton(service, (provider) => func());
    }
}

public sealed class TypeResolver : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _provider;

    public TypeResolver(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public object? Resolve(Type? type)
    {
        if (type == null)
        {
            return null;
        }

        return _provider.GetService(type);
    }

    public void Dispose()
    {
        if (_provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
