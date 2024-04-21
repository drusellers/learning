namespace CoreSecurity.Controllers;

using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    [HttpGet]
    [Route("/")]
    public IActionResult Get()
    {
        return View("Home");
    }
}