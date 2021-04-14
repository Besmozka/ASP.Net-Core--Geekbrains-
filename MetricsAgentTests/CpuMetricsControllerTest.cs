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
        private CpuMetricsController _controller;

        private Mock<ILogger<CpuMetricsController>> _mockLogger;

        private Mock<ICpuMetricsRepository> _mockRepository;

        private Mock<IMapper> _mockMapper;

        private Random _random = new Random();
        public CpuMetricsControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<CpuMetricsController>>();
            _mockRepository = new Mock<ICpuMetricsRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CpuMetricsController(_mockLogger.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void Call_GetCpuMetricsTimeInterval_From_Controller()
        {
            // ������������� �������� ��������
            // � �������� ����������� ��� � ����������� �������� CpuMetric ������
            _mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<CpuMetric>() 
                { 
                    new CpuMetric() 
                    { 
                        Id = _random.Next(),
                        Time = DateTimeOffset.FromUnixTimeSeconds(_random.Next()),
                        Value = _random.Next()
                    } 
                });

            var fromTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(50, 100));
            // ��������� �������� �� �����������
            var resultGetCpuMetricsTimeInterval = _controller.GetCpuMetricsTimeInterval(fromTime, toTime);

            // ��������� �������� �� ��, ��� ���� ������� ����������
            // ������������� �������� ����� Create ����������� � ������ ����� ������� � ���������
            _mockRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsTimeInterval);
        }

        [Fact]
        public void Call_GetCpuMetricsByPercentileTimeInterval_From_Controller()
        {
            _mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<CpuMetric>()
                {
                    new CpuMetric()
                    {
                        Id = _random.Next(),
                        Time = DateTimeOffset.FromUnixTimeSeconds(_random.Next()),
                        Value = _random.Next()
                    }
                });

            var fromTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(50, 100));
            Percentile percentile = (Percentile)_random.Next(1, 4);

            var resultGetCpuMetricsByPercentileTimeInterval = _controller.GetCpuMetricsByPercentileTimeInterval(fromTime, toTime, percentile);

            _mockRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsByPercentileTimeInterval);
        }
    }
}
