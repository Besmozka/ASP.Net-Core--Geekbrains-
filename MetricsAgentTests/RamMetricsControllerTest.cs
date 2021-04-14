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
    public class RamControllerUnitTests
    {
        private RamMetricsController _controller;

        private Mock<ILogger<RamMetricsController>> _mockLogger;

        private Mock<IRamMetricsRepository> _mockRepository;

        private Mock<IMapper> _mockMapper;

        private Random _random = new Random();
        public RamControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<RamMetricsController>>();
            _mockRepository = new Mock<IRamMetricsRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new RamMetricsController(_mockLogger.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetAvailableSize_ReturnsOk()
        {
            _mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                  .Returns(new List<RamMetric>()
                  {
                    new RamMetric()
                    {
                        Id = _random.Next(),
                        Time = DateTimeOffset.FromUnixTimeSeconds(_random.Next()),
                        Value = _random.Next()
                    }
                  });

            var resultGetRamAvailableSize = _controller.GetRamAvailableSize();

            _mockRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetRamAvailableSize);
        }
    }
}
