namespace CoreDatabase;

using System.Reflection;
using Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Models;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        await Task.Yield();

        // var template = "[{Level:u3} {SourceContext}] {Message:lj}{NewLine}{Exception}";
        var template = "[{Level:u3}] {Message:lj}{NewLine}{Exception}";
        using var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            // You can see all of the logging payload with JSON
            // .WriteTo.Console(new RenderedCompactJsonFormatter())
            .WriteTo.Console(outputTemplate: template, theme: AnsiConsoleTheme.Code)
            // .WriteTo.Console()
            .CreateLogger();
        Log.Logger = logger;

        var host = Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureServices((hostBuilder, services) =>
            {
                var conn = hostBuilder.Configuration.GetConnectionString("Learning");
                services.AddDbContext<LearningDbContext>(builder =>
                    builder.UseNpgsql(conn, m =>
                    {
                        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        m.MigrationsHistoryTable($"__{nameof(LearningDbContext)}");
                    }));
            })
            .Build();


        // set up the data
        using(var scope = host.Services.CreateScope())
        {
            var cxt = scope.ServiceProvider.GetRequiredService<LearningDbContext>();

            var user = new User("Dru");
            var message = new Message();
            message.Payload = "NEAT";
            user.Messages.Add(message);

            await cxt.AddAsync(user);
            await cxt.SaveChangesAsync();
        }

        // delete and add a line
        using (var scope = host.Services.CreateScope())
        {
            var cxt = scope.ServiceProvider.GetRequiredService<LearningDbContext>();
            var user = await cxt.Users.Where(x => x.Name == "Dru")
                .Include(x => x.Messages)
                .FirstAsync();

            user.Messages.Clear();
            var message = new Message();
            message.Payload = "NEAT2";
            user.Messages.Add(message);
            cxt.Update(user);
            await cxt.SaveChangesAsync();

        }

        // await EnsureChecks(host);
        // DemoEfcoreDebug(host);
        // await IdentityMap(host);
        // await DemonstrateUnionQueries(host);
        // await DemonstrateUniqueIndexes(host);
        // await SaveAGraph(host);

        return 0;
    }

    static async Task EnsureChecks(IHost host)
    {
        // Ensure Test (non exist)
        using (var scope = host.Services.CreateScope())
        {
            var cxt = scope.ServiceProvider.GetRequiredService<LearningDbContext>();
            await cxt.Set<Queue>().Where(x => x.Name == "Ensure")
                .ExecuteDeleteAsync();
            await cxt.Set<Queue>().Where(x => x.Name == "Ensure-Exist")
                .ExecuteDeleteAsync();
            var queue1 = new Queue()
            {
                Name = "Ensure-Exist"
            };
            await cxt.AddAsync(queue1);
            await cxt.SaveChangesAsync();
        }

        using (var scope = host.Services.CreateScope())
        {
            var cxt = scope.ServiceProvider.GetRequiredService<LearningDbContext>();
            var queue1 = new Queue()
            {
                Name = "Ensure"
            };
            await cxt.AddAsync(queue1);
            await cxt.SaveChangesAsync();

            cxt.Update(queue1);
            await cxt.SaveChangesAsync();
        }

        // Ensure Test (exist)
        using (var scope = host.Services.CreateScope())
        {
            var cxt = scope.ServiceProvider.GetRequiredService<LearningDbContext>();
            var queue1 = await cxt.Set<Queue>().Where(x => x.Name == "Ensure-Exist")
                .FirstAsync();

            cxt.Update(queue1);
            await cxt.SaveChangesAsync();
        }
    }

    static void DemoEfcoreDebug(IHost host)
    {
        using (var debugScope =  host.Services.CreateScope())
        using (var debugCxt =  debugScope.ServiceProvider.GetRequiredService<LearningDbContext>())
        {
            Console.WriteLine(debugCxt.Model.ToDebugString());
            Console.WriteLine("");
            var designModel = debugCxt.GetService<IDesignTimeModel>();
            Console.WriteLine(designModel.Model.ToDebugString());
        }
    }

    /// <summary>
    /// Can we save a graph of objects using a single add and save (yes, we can)
    /// - this works for both INSERT and UPDATE
    /// </summary>
    static async Task SaveAGraph(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var cxt = scope.ServiceProvider.GetRequiredService<LearningDbContext>();

        var user = new User("Dru");
        var message = new Message();
        message.Payload = "NEAT";
        user.Messages.Add(message);

        await cxt.AddAsync(user);

        var entries = cxt.ChangeTracker.Entries().ToList();

        await cxt.SaveChangesAsync();

        var count = await cxt.Entry(user).Collection(p => p.Messages).Query().CountAsync();

        await cxt.Users
            .Where(u => u.Id == user.Id)
            .ExecuteUpdateAsync(n => n.SetProperty(p => p.MessageCount, _ => count));
    }

    static async Task DemonstrateUnionQueries(IHost host)
    {
        // using var scope = host.Services.CreateScope();
        // var cxt = scope.ServiceProvider.GetRequiredService<LearningDbContext>();
        //
        // var sql = cxt.SubModelAs.Select(x => new { Name = x.Name })
        //     .Concat(cxt.SubModelBs.Select(x => new { Name = x.Name}))
        //     .ToQueryString();
        // Console.WriteLine(sql);
        //
        // var x = await cxt.SubModelAs.Select(x => new { Name = x.Name })
        //     .Concat(cxt.SubModelBs.Select(x => new { Name = x.Name}))
        //     .ToListAsync();
    }

    static async Task DemonstrateUniqueIndexes(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var cxt = scope.ServiceProvider.GetRequiredService<LearningDbContext>();
            var queue1 = new Queue()
            {
                Name = "ABC"
            };
            await cxt.AddAsync(queue1);
            await cxt.SaveChangesAsync();
        }

        using (var scope = host.Services.CreateScope())
        {

            var cxt = scope.ServiceProvider.GetRequiredService<LearningDbContext>();
            var queue1 = new Queue()
            {
                Name = "ABC"
            };
            await cxt.AddAsync(queue1);

            try
            {
                await cxt.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    /// <summary>
    /// Can we find an object, before we save it?
    /// - YES
    /// </summary>
    /// <param name="host"></param>
    static async Task IdentityMap(IHost host)
    {
        // using var scope = host.Services.CreateScope();
        //
        // var cxt = scope.ServiceProvider.GetRequiredService<LearningDbContext>();
        // var user = new User("Find Me");
        //
        // await cxt.AddAsync(user);
        //
        // var foundYouBeforeSave = cxt.Users.FirstOrDefault(u => u.Name == "Find Me"); // true
        // Debug.Assert(foundYouBeforeSave != null, "We should find this.");
        //
        // await cxt.SaveChangesAsync();
        //
        // var foundYouAfterSave = cxt.Users.FirstOrDefault(u => u.Name == "Find Me"); // true
        //
        // int i = 0;
    }
}
