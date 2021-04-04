using AutoMapper;
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
        private CpuMetricsController controller;

        private Mock<ILogger<CpuMetricsController>> mockLogger;

        private Mock<ICpuMetricsRepository> mockRepository;

        private Mock<IMapper> mockMapper;
        public CpuMetricsControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<CpuMetricsController>>();
            mockRepository = new Mock<ICpuMetricsRepository>();
            mockMapper = new Mock<IMapper>();
            controller = new CpuMetricsController(mockLogger.Object, mockRepository.Object, mockMapper.Object);
        }

        [Fact]
        public void Call_AllMethods_From_Controller()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            mockRepository.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();

            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50, 100));
            Percentile percentile = (Percentile)random.Next(1, 4);
            // выполняем действие на контроллере
            var resultGetCpuMetricsTimeInterval = controller.GetCpuMetricsTimeInterval(fromTime, toTime);
            var resultGetCpuMetricsByPercentileTimeInterval = controller.GetCpuMetricsByPercentileTimeInterval(fromTime, toTime, percentile);

            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            mockRepository.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.Exactly(4));
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsTimeInterval);
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsByPercentileTimeInterval);

        }
    }
}
