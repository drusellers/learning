// See https://aka.ms/new-console-template for more information

using CoreLogging;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using SerilogTimings;
using SerilogTimings.Extensions;

var levelSwitch = new LoggingLevelSwitch();
using var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.ControlledBy(levelSwitch)
    // You can see all of the logging payload with JSON
    // .WriteTo.Console(new RenderedCompactJsonFormatter())
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    // .WriteTo.Console()
    .CreateLogger();
Log.Logger = logger;


var x = logger.ForContext<Thing>();
logger.Information("ABC");
using var cxt = LogContext.PushProperty("B", "A");
x.Information("HMM");


// binding serilog to the dotnet framework
var lf = LoggerFactory.Create(b =>
{
    b.AddSerilog(dispose:true);
});

var ml = lf.CreateLogger<Thing>();
ml.LogInformation("YUP");

// Serilog Timings
using (Operation.Time("ABC"))
{
    Thread.Sleep(2);
}

// Serilog Timings (via Extension on log)
using (x.TimeOperation("NEAT"))
{
    Thread.Sleep(3);
}

// checking how \n is handled
ml.LogInformation("I hack you {Str}", "abc\ndef");

// Serilog Context
using var _ = LogContext.PushProperty("AccountId", "acct");

// custom scope extention
using var scope = ml.BeginOrganizationScope("name");

// This is the source generator logging
ml.CheckIt("Nuts");
