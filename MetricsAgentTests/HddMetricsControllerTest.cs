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
    public class HddControllerUnitTests
    {
        private HddMetricsController _controller;

        private Mock<ILogger<HddMetricsController>> _mockLogger;

        private Mock<IHddMetricsRepository> _mockRepository;

        private Mock<IMapper> _mockMapper;

        private Random _random = new Random();
        public HddControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<HddMetricsController>>();
            _mockRepository = new Mock<IHddMetricsRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new HddMetricsController(_mockLogger.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetSizeLeft_ReturnsOk()
        {
            _mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<HddMetric>()
                {
                    new HddMetric()
                    {
                        Id = _random.Next(),
                        Time = DateTimeOffset.FromUnixTimeSeconds(_random.Next()),
                        Value = _random.Next()
                    }
                });

            var resultGetHddSizeLeft = _controller.GetHddSizeLeft();

            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            _mockRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetHddSizeLeft);
        }
    }
}
