namespace CoreDatabase;

using Db;
using Microsoft.EntityFrameworkCore;

public static class Migrator
{
    // --migrate=true
    public static bool ShouldRunMigrations(IHost host)
    {
        // var cfg = host.Services.GetRequiredService<IConfiguration>();
        // var x = cfg["migrate"];
        //
        // if (x == null)
        //     return false;
        //
        // var result = false;
        // if (bool.TryParse(x, out result))
        // {
        //     return result;
        // }
        //
        // return false;
        return true;
    }

    public static async Task Run(IHost host)
    {
        if (!ShouldRunMigrations(host))
        {
            Console.WriteLine("Skipping Migrations");
            return;
        }


        Console.WriteLine("Running migrations");
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LearningDbContext>();

        await db.Database.MigrateAsync();
    }
}