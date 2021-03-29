using MetricsAgent.Controllers;
using MetricsAgent.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class CpuControllerUnitTests
    {
        private CpuMetricsController controller;

        public CpuControllerUnitTests()
        {
            controller = new CpuMetricsController();
        }

        [Fact]
        public void GetMetricsTimeInterval_ReturnsOk()
        {
            //Arrange
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            //Act
            var result = controller.GetCpuMetricsTimeInterval(fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetMetricsByPercentileTimeInterval_ReturnsOk()
        {
            //Arrange
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            var random = new Random();
            Percentile percentile = (Percentile)random.Next(0, 4);

            //Act
            var result = controller.GetCpuMetricsByPercentileTimeInterval(fromTime, toTime, percentile);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
