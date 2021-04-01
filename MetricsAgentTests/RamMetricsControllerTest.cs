using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class RamControllerUnitTests
    {
        private RamMetricsController controller;

        private ILogger<RamMetricsController> logger;
        public RamControllerUnitTests()
        {
            controller = new RamMetricsController(logger);
        }

        [Fact]
        public void GetAvailableSize_ReturnsOk()
        {
            //Arrange

            //Act
            var result = controller.GetRamAvailableSize();

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
