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

        public CpuMetricsControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<CpuMetricsController>>();
            mockRepository = new Mock<ICpuMetricsRepository>();
            controller = new CpuMetricsController(mockLogger.Object, mockRepository.Object);
        }

        [Fact]
        public void Call_AllMethods_From_Repository()
        {
            // ������������� �������� ��������
            // � �������� ����������� ��� � ����������� �������� CpuMetric ������
            mockRepository.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();

            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50, 100));
            Percentile percentile = (Percentile)random.Next(1, 4);
            // ��������� �������� �� �����������
            var resultGetCpuMetricsTimeInterval = controller.GetCpuMetricsTimeInterval(fromTime, toTime);
            var resultGetCpuMetricsByPercentileTimeInterval = controller.GetCpuMetricsByPercentileTimeInterval(fromTime, toTime, percentile);

            // ��������� �������� �� ��, ��� ���� ������� ����������
            // ������������� �������� ����� Create ����������� � ������ ����� ������� � ���������
            mockRepository.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.Exactly(4));
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsTimeInterval);
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsByPercentileTimeInterval);

        }
    }
}
