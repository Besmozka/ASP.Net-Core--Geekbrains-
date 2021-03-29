using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class HddControllerUnitTests
    {
        private HddMetricsController controller;

        public HddControllerUnitTests()
        {
            controller = new HddMetricsController();
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
