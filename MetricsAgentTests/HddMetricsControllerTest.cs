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
    public class HddControllerUnitTests
    {
        private HddMetricsController _controller;

        private Mock<ILogger<HddMetricsController>> _mockLogger;
        private Mock<IHddMetricsRepository> _mockRepository;
        public HddControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<HddMetricsController>>();
            _mockRepository = new Mock<IHddMetricsRepository>();
            _controller = new HddMetricsController(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public void GetSizeLeft_ReturnsOk()
        {
            //Arrange
            _mockRepository.Setup(repository => repository.Create(It.IsAny<HddMetric>())).Verifiable();
            //Act
            var result = _controller.GetHddSizeLeft();

            // Assert
            _mockRepository.Verify(repository => repository.Create(It.IsAny<HddMetric>()), Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
