namespace CoreSecurity.Authentication;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;

public class CustomGithubAuthenticationEvents : OAuthEvents
{
    readonly ILogger<CustomGithubAuthenticationEvents> _logger;

    public CustomGithubAuthenticationEvents(ILogger<CustomGithubAuthenticationEvents> logger)
    {
        _logger = logger;
    }

    public override Task CreatingTicket(OAuthCreatingTicketContext context)
    {
        var githubId =
            context.Identity?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

        if (githubId != null)
        {
            // ok, we have someone

            // this gets called when the user is redirected back to us
            if (context.AccessToken is { })
            {
                context.Identity?.AddClaim(new Claim("access_token", context.AccessToken));
            }

            // TODO: dig into our database here
            // TODO: using (github, 6355) find the right user
            // TODO: set the user id and the account id
            // TODO: read up on the impersonation
            // TODO: what do we do with new users
            if (context.Identity?.Name == "drusellers")
            {
                context.Identity.AddClaim(new Claim("Employee","yup"));
            }
        }

        return Task.CompletedTask;
    }
}
