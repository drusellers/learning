using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers;

using System.Diagnostics;
using System.Text.Json;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get([FromQuery] Paging paging)
    {
        Console.WriteLine(paging);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public IActionResult Post(NestedJson el)
    {
        Console.WriteLine(el.Data);

        return Problem(statusCode: 404);
        return Ok();
    }
}

[DebuggerDisplay("{Page} .. {PerPage}")]
public class Paging
{
    [FromQuery(Name = "per_page")]
    public int? PerPage { get; set; }
    [FromQuery(Name = "page")]
    public int? Page { get; set; }

    public override string ToString()
    {
        return $"Page = {Page} -- PerPage = {PerPage}";
    }
}
public class NestedJson
{
    public JsonElement Data { get; set; }
}
