namespace CoreSecurity.Authentication;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
{
    readonly ILogger<CustomCookieAuthenticationEvents> _logger;

    public CustomCookieAuthenticationEvents(ILogger<CustomCookieAuthenticationEvents> logger)
    {
        _logger = logger;
    }

    public override Task SigningIn(CookieSigningInContext context)
    {
        return base.SigningIn(context);
    }

    public override async Task SignedIn(CookieSignedInContext context)
    {
        _logger.LogInformation("RedirectToAccessDenied");
        await base.SignedIn(context);
    }

    public override async Task SigningOut(CookieSigningOutContext context)
    {
        _logger.LogInformation("RedirectToAccessDenied");
        await base.SigningOut(context);
    }

    public override async Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
    {
        _logger.LogInformation("RedirectToAccessDenied");
        await base.RedirectToLogout(context);
    }

    public override async Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
    {
        _logger.LogInformation("RedirectToAccessDenied");
        await base.RedirectToLogin(context);
    }

    public override async Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
    {
        _logger.LogInformation("RedirectToAccessDenied");
        await base.RedirectToAccessDenied(context);
    }

    public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        _logger.LogInformation("ValidatePrincipal");
        return Task.CompletedTask;

        // if (false)
// {
//     context.RejectPrincipal();
//
//     await context.HttpContext.SignOutAsync(
//         CookieAuthenticationDefaults.AuthenticationScheme);
// }

    }

    public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
    {
        _logger.LogInformation("Redirect");
        return Task.CompletedTask;
    }
}
