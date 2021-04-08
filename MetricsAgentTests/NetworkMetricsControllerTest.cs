using AutoMapper;
using MetricsAgent;
using MetricsAgent.DAL;
using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsAgentTests
{
    public class NetworkControllerUnitTests
    {
        private NetworkMetricsController _controller;

        private Mock<ILogger<NetworkMetricsController>> _mockLogger;

        private Mock<INetworkMetricsRepository> _mockRepository;

        private Mock<IMapper> _mockMapper;

        public NetworkControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<NetworkMetricsController>>();
            _mockRepository = new Mock<INetworkMetricsRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new NetworkMetricsController(_mockLogger.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetMetricsTimeInterval_ReturnsOk()
        {
            //Arrange
            _mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<NetworkMetric>()); ;
            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50, 100));

            //Act
            var result = _controller.GetNetworkMetricsTimeInterval(fromTime, toTime);

            // Assert
            _mockRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()),
                Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
