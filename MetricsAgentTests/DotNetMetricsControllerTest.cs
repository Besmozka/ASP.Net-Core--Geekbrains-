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
    public class DotNetControllerUnitTests
    {
        private DotNetMetricsController controller;

        private Mock<ILogger<DotNetMetricsController>> mockLogger;

        private Mock<IDotNetMetricsRepository> mockRepository;

        private Mock<IMapper> mockMapper;
        public DotNetControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<DotNetMetricsController>>();
            mockRepository = new Mock<IDotNetMetricsRepository>();
            mockMapper = new Mock<IMapper>();
            controller = new DotNetMetricsController(mockLogger.Object, mockRepository.Object, mockMapper.Object);
        }

        [Fact]
        public void GetErrorsTimeInterval_ReturnsOk()
        {
            //ArrangeRandom 
            mockRepository.Setup(repository => repository.Create(It.IsAny<DotNetMetric>())).Verifiable();

            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50, 100));

            //Act
            var result = controller.GetDotNetErrorsTimeInterval(fromTime, toTime);


            // Assert
            mockRepository.Verify(repository => repository.Create(It.IsAny<DotNetMetric>()), Times.Exactly(2));
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
