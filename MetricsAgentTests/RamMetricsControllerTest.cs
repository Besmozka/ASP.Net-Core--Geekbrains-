using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class RamControllerUnitTests
    {
        private RamMetricsController controller;

        public RamControllerUnitTests()
        {
            controller = new RamMetricsController();
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
