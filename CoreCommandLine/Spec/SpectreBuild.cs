namespace CoreCommandLine.Spec;

using Spectre.Console;
using Spectre.Console.Cli;

public static class SpectreBuild
{
    public static int Go(string[] args)
    {
        var app = new CommandApp();
        app.Configure(cfg =>
        {
            cfg.AddCommand<ACommand>("a");
            cfg.AddCommand<BCommand>("b");
        });

        return app.Run(args);
    }
}

public class ACommand: Command
{
    public override int Execute(CommandContext context)
    {
        throw new NotImplementedException();
    }
}

public class BArgs : CommandSettings
{

}

public class BCommand : Command<BArgs>
{
    public ValidationResult Validate(CommandContext context, CommandSettings settings)
    {
        throw new NotImplementedException();
    }

    public override int Execute(CommandContext context, BArgs settings)
    {
        throw new NotImplementedException();
    }
}
