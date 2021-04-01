using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class HddControllerUnitTests
    {
        private HddMetricsController controller;

        private ILogger<HddMetricsController> logger;
        public HddControllerUnitTests()
        {
            controller = new HddMetricsController(logger);
        }

        [Fact]
        public void GetSizeLeft_ReturnsOk()
        {
            //Arrange

            //Act
            var result = controller.GetHddSizeLeft();

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
