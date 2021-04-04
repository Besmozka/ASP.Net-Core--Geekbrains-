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
    public class RamControllerUnitTests
    {
        private RamMetricsController controller;

        private Mock<ILogger<RamMetricsController>> mockLogger;

        private Mock<IRamMetricsRepository> mockRepository;

        private Mock<IMapper> mockMapper;
        public RamControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<RamMetricsController>>();
            mockRepository = new Mock<IRamMetricsRepository>();
            mockMapper = new Mock<IMapper>();
            controller = new RamMetricsController(mockLogger.Object, mockRepository.Object, mockMapper.Object);
        }

        [Fact]
        public void GetAvailableSize_ReturnsOk()
        {
            //Arrange
            mockRepository.Setup(repository => repository.Create(It.IsAny<RamMetric>())).Verifiable();
            //Act
            var result = controller.GetRamAvailableSize();

            // Assert
            mockRepository.Verify(repository => repository.Create(It.IsAny<RamMetric>()), Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
