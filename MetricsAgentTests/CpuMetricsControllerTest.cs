using AutoFixture;
using AutoMapper;
using Core;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using MetricsAgent.SQLSettingsProvider;

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
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mp =>
                mp.CreateMap<CpuMetric, CpuMetricDto>());
            IMapper mapper = mapperConfiguration.CreateMapper();
            _controller = new CpuMetricsController(_mockLogger.Object, _mockRepository.Object, mapper);
        }


        [Fact]
        public void Call_GetCpuMetricsTimeInterval_From_Controller()
        {
            Fixture fixture = new Fixture();
            var returnList = fixture.Create<List<CpuMetric>>();

            _mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(returnList);

            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(500));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(500, 1000));

            var resultGetCpuMetricsTimeInterval = (OkObjectResult)_controller.GetCpuMetricsTimeInterval(fromTime, toTime);
            var returnListDto = (AllMetricsResponse<CpuMetricDto>)resultGetCpuMetricsTimeInterval.Value;

            _mockRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsTimeInterval);
            for (int i = 0; i < returnList.Count; i++)
            {
                Assert.Equal(returnList[i].Id, returnListDto.Metrics[i].Id);
                Assert.Equal(returnList[i].Time, returnListDto.Metrics[i].Time);
                Assert.Equal(returnList[i].Value, returnListDto.Metrics[i].Value);
            }
        }
    }
}
