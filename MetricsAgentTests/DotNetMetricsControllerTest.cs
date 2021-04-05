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
        private DotNetMetricsController _controller;

        private Mock<ILogger<DotNetMetricsController>> _mockLogger;

        private Mock<IDotNetMetricsRepository> _mockRepository;

        private Mock<IMapper> _mockMapper;
        public DotNetControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<DotNetMetricsController>>();
            _mockRepository = new Mock<IDotNetMetricsRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new DotNetMetricsController(_mockLogger.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetErrorsTimeInterval_ReturnsOk()
        {
            //ArrangeRandom 
            _mockRepository.Setup(repository => repository.Create(It.IsAny<DotNetMetric>())).Verifiable();

            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50, 100));

            //Act
            var result = _controller.GetDotNetErrorsTimeInterval(fromTime, toTime);


            // Assert
            _mockRepository.Verify(repository => repository.Create(It.IsAny<DotNetMetric>()), Times.Exactly(2));
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
