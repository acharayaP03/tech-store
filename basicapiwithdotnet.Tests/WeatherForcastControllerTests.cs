

using basicapiwithdotnet.Controllers;

namespace basicapiwithdotnet.Tests;

public class WeatherForcastControllerTests
{
    [Fact]
    public void GetFiveDayForecast_ReturnsFiveForecasts()
    {
        var controller = new WeatherForcastController();

        var result = controller.GetFiveDayForecast();

        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
    }
}