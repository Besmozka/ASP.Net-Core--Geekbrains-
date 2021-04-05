using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class DotNetControllerUnitTests
    {
        private DotNetMetricsController _controller;

        private Mock<ILogger<DotNetMetricsController>> _mockLogger;
        public DotNetControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<DotNetMetricsController>>();
            _controller = new DotNetMetricsController(_mockLogger.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            var agentId = 1;
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            //Act
            var result = _controller.GetDotNetMetricsFromAgent(agentId, fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
