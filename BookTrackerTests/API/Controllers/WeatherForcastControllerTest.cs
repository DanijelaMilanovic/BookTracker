using API.Controllers;
using Microsoft.Extensions.Logging;

namespace API.Tests
{
    public class WeatherForecastControllerTests
    {
        [Fact]
        public void Get_ReturnsWeatherForecasts()
        {
            var controller = new WeatherForecastController(new LoggerFactory().CreateLogger<WeatherForecastController>());

            var result = controller.Get();

            Assert.NotNull(result);

            var forecastList = result.ToList();
            Assert.Equal(5, forecastList.Count);

            foreach (var forecast in forecastList)
            {
                Assert.True(forecast.Date >= DateTime.Now.Date);
                Assert.InRange(forecast.TemperatureC, -20, 55);
            }
        }
    }
}
