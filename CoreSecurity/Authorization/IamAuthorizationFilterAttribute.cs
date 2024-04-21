namespace CoreSecurity;

using Microsoft.AspNetCore.Mvc.Filters;

public class IamAuthorizationFilterAttribute : ActionFilterAttribute, IAuthorizationFilter
{
   

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // context.HttpContext.RequestServices.GetRequiredService<ILogger>();
        
        var ad = context.ActionDescriptor;
        var i = ad.Id;
    }
}