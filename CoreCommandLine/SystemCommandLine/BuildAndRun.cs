namespace CoreCommandLine.SystemCommandLine;

using System.CommandLine;
using System.Diagnostics;

public class BuildAndRun
{
    public static int Go(string[] args)
    {
        var migrationOption = new Option<bool>("--migrate");

        var rootCommand = new RootCommand
        {
            migrationOption
        };

        var subCommand = new Command("subcommand");
        subCommand.SetHandler(() =>
        {
            Console.WriteLine("HI");
        });

        rootCommand.AddCommand(subCommand);

        rootCommand.Description = "HI";
        rootCommand.SetHandler(async (bool b, CancellationToken ct) =>
        {
            if(b)
                Console.WriteLine("Hello, World!");

            var psi = new ProcessStartInfo
            {
                FileName = "sh",
                Arguments = "test.sh",
            };
            var p = new Process
            {
                StartInfo = psi,

            };
            p.Start();
            await p.WaitForExitAsync(ct);

            Console.WriteLine("Exit {0}", p.ExitCode);
        }, migrationOption);


        return rootCommand.Invoke(args);
    }
}
