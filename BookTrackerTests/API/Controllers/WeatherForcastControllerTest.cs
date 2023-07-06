using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Xunit;

namespace API.Tests
{
    public class WeatherForecastControllerTests
    {
        [Fact]
        public void Get_ReturnsWeatherForecasts()
        {
            // Arrange
            var controller = new WeatherForecastController(new LoggerFactory().CreateLogger<WeatherForecastController>());

            // Act
            var result = controller.Get();

            // Assert
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
