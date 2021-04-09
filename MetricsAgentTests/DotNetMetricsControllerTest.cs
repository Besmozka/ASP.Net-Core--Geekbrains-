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
    public class DotNetControllerUnitTests
    {
        private DotNetMetricsController _controller;

        private Mock<ILogger<DotNetMetricsController>> _mockLogger;

        private Mock<IDotNetMetricsRepository> _mockRepository;

        private Mock<IMapper> _mockMapper;

        private Random _random;
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
            _mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<DotNetMetric>()); ;
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(50, 100));

            var result = _controller.GetDotNetErrorsTimeInterval(fromTime, toTime);

            _mockRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()),
                Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
