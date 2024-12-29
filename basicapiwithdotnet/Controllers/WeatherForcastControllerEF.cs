


using Microsoft.AspNetCore.Mvc;

namespace basicapiwithdotnet.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForcastControllerEF : ControllerBase
{
    private readonly string[] _summary = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet("", Name = "GetFiveDayForecastEF")]
    public IEnumerable<WeatherForecastEF> GetFiveDayForecastEF()
    {
        var forecast = Enumerable.Range(1, 10).Select(index =>
        new WeatherForecastEF
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            _summary[Random.Shared.Next(_summary.Length)]
        ))
        .ToArray();

        return forecast;
    }
}

public record WeatherForecastEF(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}