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
    public class HddControllerUnitTests
    {
        private HddMetricsController controller;

        private Mock<ILogger<HddMetricsController>> mockLogger;

        private Mock<IHddMetricsRepository> mockRepository;

        private Mock<IMapper> mockMapper;
        public HddControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<HddMetricsController>>();
            mockRepository = new Mock<IHddMetricsRepository>();
            mockMapper = new Mock<IMapper>();
            controller = new HddMetricsController(mockLogger.Object, mockRepository.Object, mockMapper.Object);
        }

        [Fact]
        public void GetSizeLeft_ReturnsOk()
        {
            //Arrange
            mockRepository.Setup(repository => repository.Create(It.IsAny<HddMetric>())).Verifiable();
            //Act
            var result = controller.GetHddSizeLeft();

            // Assert
            mockRepository.Verify(repository => repository.Create(It.IsAny<HddMetric>()), Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
