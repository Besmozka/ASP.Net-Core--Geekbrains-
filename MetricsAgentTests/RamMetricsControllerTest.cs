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
    public class RamControllerUnitTests
    {
        private RamMetricsController _controller;

        private Mock<ILogger<RamMetricsController>> _mockLogger;

        private Mock<IRamMetricsRepository> _mockRepository;
        public RamControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<RamMetricsController>>();
            _mockRepository = new Mock<IRamMetricsRepository>();
            _controller = new RamMetricsController(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public void GetAvailableSize_ReturnsOk()
        {
            //Arrange
            _mockRepository.Setup(repository => repository.Create(It.IsAny<RamMetric>())).Verifiable();
            //Act
            var result = _controller.GetRamAvailableSize();

            // Assert
            _mockRepository.Verify(repository => repository.Create(It.IsAny<RamMetric>()), Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
