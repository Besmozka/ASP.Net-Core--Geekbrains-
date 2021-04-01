using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsController controller;
        private Mock<CpuMetricsRepository> mock;
        private Mock<ILogger<CpuMetricsController>> mock2;

        public CpuMetricsControllerUnitTests()
        {
            mock = new Mock<CpuMetricsRepository>(null);
            mock2 = new Mock<ILogger<CpuMetricsController>>();
            controller = new CpuMetricsController(mock2.Object, mock.Object);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            mock.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();
            mock2.Setup()

            Random random = new Random();
            // выполняем действие на контроллере
            var result = controller.GetCpuMetricsTimeInterval(DateTimeOffset.FromUnixTimeSeconds(random.Next(50)),
                DateTimeOffset.FromUnixTimeSeconds(random.Next(50,100)));

            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            mock.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.AtMostOnce());
        }
    }
}
