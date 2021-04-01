using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class NetworkControllerUnitTests
    {
        private NetworkMetricsController controller;

        private ILogger<NetworkMetricsController> logger;
        public NetworkControllerUnitTests()
        {
            controller = new NetworkMetricsController(logger);
        }

        [Fact]
        public void GetMetricsTimeInterval_ReturnsOk()
        {
            //Arrange
            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50, 100));

            //Act
            var result = controller.GetNetworkMetricsTimeInterval(fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
