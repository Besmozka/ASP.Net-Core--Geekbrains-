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
        public HddControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<HddMetricsController>>();
            mockRepository = new Mock<IHddMetricsRepository>();
            controller = new HddMetricsController(mockLogger.Object, mockRepository.Object);
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
