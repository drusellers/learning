namespace SampleDbTransport;

using MassTransit;
using MassTransit.SqlTransport.PostgreSql;

public class Startup
{

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(o =>
        {

        });

        services.AddOptions<SqlTransportOptions>()
            .Configure(options =>
        {
            options.Host = "localhost";
            options.Database = "learning_development";
            options.Schema = "transport"; // the schema for the transport-related tables, etc.
            options.Role = "transport";   // the role to assign for all created tables, functions, etc.
            options.Username = "learning";  // the application-level credentials to use
            options.Password = "learning";
            options.AdminUsername = "drusellers"; // the admin credentials to create the tables, etc.
            options.AdminPassword = "password";
        });

        services.AddPostgresMigrationHostedService(create: true, delete: false);

        services.AddMassTransit(cfg =>
        {
            // needed so we can pull it from the container
            cfg.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(false));

            cfg.AddConsumers(typeof(Startup).Assembly);
            cfg.AddActivities(typeof(Startup).Assembly);


            cfg.UsingPostgres((context, transport) =>
            {
                transport.ConfigureEndpoints(context);
            });
        });

    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // !! Order Matters here

        app.UseRouting();

        // Demo system (disable)
        // app.UseAuthentication();
        // app.UseAuthorization();

        app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        app.UseEndpoints(ep =>
        {
            ep.MapControllers();
        });
    }
}
