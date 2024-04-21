namespace CoreSecurity.Authentication;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

public class CustomGoogleAuthenticationEvents : OAuthEvents
{
    readonly ILogger<CustomGithubAuthenticationEvents> _logger;

    public CustomGoogleAuthenticationEvents(ILogger<CustomGithubAuthenticationEvents> logger)
    {
        _logger = logger;
    }

    public override Task CreatingTicket(OAuthCreatingTicketContext context)
    {
        return Task.CompletedTask;
    }

    public override async Task TicketReceived(TicketReceivedContext context)
    {
        _logger.LogInformation("TicketReceived: Google");
        await base.TicketReceived(context);
    }

    public override async Task RedirectToAuthorizationEndpoint(RedirectContext<OAuthOptions> context)
    {
        await base.RedirectToAuthorizationEndpoint(context);
    }

    public override async Task AccessDenied(AccessDeniedContext context)
    {
        await base.AccessDenied(context);
    }

    public override async Task RemoteFailure(RemoteFailureContext context)
    {
        await base.RemoteFailure(context);
    }


}
