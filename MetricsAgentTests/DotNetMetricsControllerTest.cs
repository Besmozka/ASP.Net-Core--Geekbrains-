using AutoFixture;
using AutoMapper;
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

namespace MetricsAgentTests
{
    public class DotNetControllerUnitTests
    {
        private DotNetMetricsController _controller;

        private Mock<ILogger<DotNetMetricsController>> _mockLogger;

        private Mock<IDotNetMetricsRepository> _mockRepository;
        public DotNetControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<DotNetMetricsController>>();
            _mockRepository = new Mock<IDotNetMetricsRepository>();
            var mapperConfiguration = new MapperConfiguration(mp => mp.CreateMap<DotNetMetric, DotNetMetricDto>());
            var mapper = mapperConfiguration.CreateMapper();
            _controller = new DotNetMetricsController(_mockLogger.Object, _mockRepository.Object, mapper);
        }

        [Fact]
        public void GetErrorsTimeInterval_ReturnsOk()
        {
            Fixture fixture = new Fixture();
            var returnList = fixture.Create<List<DotNetMetric>>();

            _mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(returnList);

            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(50, 100));

            var resultGetDotNetErrorsTimeInterval = (OkObjectResult)_controller.GetDotNetErrorsTimeInterval(fromTime, toTime);
            var returnListDto = (AllMetricsResponse<CpuMetricDto>)resultGetDotNetErrorsTimeInterval.Value;

            _mockRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()),
                Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetDotNetErrorsTimeInterval);

            for (int i = 0; i < returnList.Count; i++)
            {
                Assert.Equal(returnList[i].Id, returnListDto.Metrics[i].Id);
                Assert.Equal(returnList[i].Time, returnListDto.Metrics[i].Time);
                Assert.Equal(returnList[i].Value, returnListDto.Metrics[i].Value);
            }
        }
    }
}
