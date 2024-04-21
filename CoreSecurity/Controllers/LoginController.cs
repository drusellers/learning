namespace CoreSecurity.Controllers;

using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("login")]
[AllowAnonymous]
public class LoginController : Controller
{

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        var schemes =  HttpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
        var s = (await schemes.GetAllSchemesAsync())
            .Where(scheme => !string.IsNullOrEmpty(scheme.DisplayName))
            .ToList();

        return View(new LoginRequest(s));
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request, [FromForm] string provider,
        [FromForm] string returnUrl)
    {
        if (!string.IsNullOrEmpty(provider))
        {
            var asyncish = await IsProviderSupportedAsync(HttpContext, provider);
            if (asyncish is false)
            {
                return BadRequest();
            }

            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.IsLocalUrl(returnUrl) ? returnUrl : "/"
            }, provider);

        }

        if (request.Username == request.Password)
        {
            var i = new GenericIdentity(request.Username, "Cookie");
            var claims = new List<Claim>();
            claims.Add(new Claim("Employee", "e"));
            claims.Add(new Claim("Customer", "e"));
            var identity = new ClaimsIdentity(i, claims);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

            return Redirect("/paid");
        }

        return View(request);
    }

    async Task<AuthenticationScheme[]> GetExternalProvidersAsync(HttpContext context)
    {
        var schemes =  context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
        return (await schemes.GetAllSchemesAsync())
            .Where(scheme => !string.IsNullOrEmpty(scheme.DisplayName))
            .ToArray();
    }

    async Task<bool> IsProviderSupportedAsync(HttpContext context, string provider)
    {
        return (await GetExternalProvidersAsync(context))
            .Any(scheme => string.Equals(scheme.Name, provider, StringComparison.OrdinalIgnoreCase));
    }



    public class LoginRequest
    {
        public LoginRequest()
        {
            Schemes = new List<AuthenticationScheme>();
        }

        public LoginRequest(IEnumerable<AuthenticationScheme> schemes)
        {
            Schemes = schemes;
        }

        public IEnumerable<AuthenticationScheme> Schemes { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; } = "";
    }
}
