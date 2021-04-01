using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class DotNetControllerUnitTests
    {
        private DotNetMetricsController controller;

        private ILogger<DotNetMetricsController> logger;

        public DotNetControllerUnitTests()
        {
            controller = new DotNetMetricsController(logger);
        }

        [Fact]
        public void GetErrorsTimeInterval_ReturnsOk()
        {
            //ArrangeRandom 
            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50, 100));

            //Act
            var result = controller.GetDotNetErrorsTimeInterval(fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
