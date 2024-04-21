namespace CoreSecurity.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class ProtectedController : Controller
{
    [HttpGet]
    [Route("/paid")]
    [Authorize]
    public IActionResult Paying()
    {
        return View("paid");
    }

    [HttpGet]
    [Route("/iam")]
    [IamAuthorizationFilter]
    public IActionResult Custom()
    {
        return View("admin");
    }


    // GitHub adds this claim
    [Authorize(Policy = "IsEmployee")]
    [HttpGet]
    [Route("/admin")]
    public IActionResult Admin()
    {
        return View("admin");
    }

    [HttpGet]
    [Route("/denied")]
    public IActionResult Denied()
    {
        return View("denied");
    }
}
