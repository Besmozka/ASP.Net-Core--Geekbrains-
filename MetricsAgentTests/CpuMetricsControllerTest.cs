using EnumsLibrary;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsAgentTests
{
    public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsController _controller;

        private Mock<ILogger<CpuMetricsController>> _mockLogger;

        private Mock<ICpuMetricsRepository> _mockRepository;

        public CpuMetricsControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<CpuMetricsController>>();
            _mockRepository = new Mock<ICpuMetricsRepository>();
            _controller = new CpuMetricsController(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public void Call_AllMethods_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _mockRepository.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();

            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50, 100));
            Percentile percentile = (Percentile)random.Next(1, 4);
            // выполняем действие на контроллере
            var resultGetCpuMetricsTimeInterval = _controller.GetCpuMetricsTimeInterval(fromTime, toTime);
            var resultGetCpuMetricsByPercentileTimeInterval = _controller.GetCpuMetricsByPercentileTimeInterval(fromTime, toTime, percentile);

            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            _mockRepository.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.Exactly(4));
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsTimeInterval);
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsByPercentileTimeInterval);

        }
    }
}
