using AutoMapper;
using MetricsAgent;
using MetricsAgent.DAL;
using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class NetworkControllerUnitTests
    {
        private NetworkMetricsController controller;

        private Mock<ILogger<NetworkMetricsController>> mockLogger;

        private Mock<INetworkMetricsRepository> mockRepository;

        private Mock<IMapper> mockMapper;

        public NetworkControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<NetworkMetricsController>>();
            mockRepository = new Mock<INetworkMetricsRepository>();
            mockMapper = new Mock<IMapper>();
            controller = new NetworkMetricsController(mockLogger.Object, mockRepository.Object, mockMapper.Object);
        }

        [Fact]
        public void GetMetricsTimeInterval_ReturnsOk()
        {
            //Arrange
            mockRepository.Setup(repository => repository.Create(It.IsAny<NetworkMetric>())).Verifiable();
            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50, 100));

            //Act
            var result = controller.GetNetworkMetricsTimeInterval(fromTime, toTime);

            // Assert
            mockRepository.Verify(repository => repository.Create(It.IsAny<NetworkMetric>()), Times.Exactly(2));
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
