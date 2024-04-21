using AspNet.Security.OAuth.GitHub;
using CoreSecurity;
using CoreSecurity.Authentication;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;

var builder = WebApplication.CreateBuilder(args);

// CERT AUTH STUFF
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.ConfigureHttpsDefaults(ssl =>
    {
        ssl.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
    });
});
builder.WebHost.UseKestrel(x =>
{
    // this is the HTTP port
    x.ListenAnyIP(3000);

    // how to get the HTTP port
});

var cfg = new AuthenticationConfig();
builder.Configuration.GetSection(AuthenticationConfig.ConfigKey).Bind(cfg);

builder.Services.AddLogging();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        // this is how we know where to redirect too. (this is for AuthenticationHandlers that are SignInHandlers)
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        // options.Cookie
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/denied";
        // options.Cookie.Domain = "twosix.io";
        options.Cookie.Name = "JWT";
        options.ReturnUrlParameter = "returnUrl";
        options.EventsType = typeof(CustomCookieAuthenticationEvents);
    })
    .AddGitHub(GitHubAuthenticationDefaults.AuthenticationScheme,options =>
    {
        options.ClientId = cfg.GitHub.ClientId;
        options.ClientSecret = cfg.GitHub.ClientSecret;
        options.Scope.Add("read:user");

        options.EventsType = typeof(CustomGithubAuthenticationEvents);
    })
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = cfg.Google.ClientId;
        options.ClientSecret = cfg.Google.ClientSecret;

        options.EventsType = typeof(CustomGoogleAuthenticationEvents);
    })
    // https://learn.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-7.0
    // .AddCertificate(CertificateAuthenticationDefaults.AuthenticationScheme, options =>
    // {
    //     options.AllowedCertificateTypes = CertificateTypes.All;
    //     options.Events.OnCertificateValidated = context =>
    //     {
    //         return Task.CompletedTask;
    //     };
    // })
    ;

builder.Services.AddScoped<CustomCookieAuthenticationEvents>();
builder.Services.AddScoped<CustomGithubAuthenticationEvents>();
builder.Services.AddScoped<CustomGoogleAuthenticationEvents>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsEmployee", policy =>
    {
        policy.RequireClaim("Employee");
    });
    options.AddPolicy("IsCustomer", policy => policy.RequireClaim("Customer"));
});

builder.Services.AddMvc();

// Registration Complete

var app = builder.Build();

// this has to be on to support the GitHub login
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.Run();
