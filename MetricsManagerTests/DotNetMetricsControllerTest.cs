using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using AutoFixture;
using AutoMapper;
using MetricsManager.DAL;
using MetricsManager.DAL.Interfaces;
using MetricsManager.Responses;
using MetricsManager.Responses.Models;
using Xunit;

namespace MetricsManagerTests
{
    public class DotNetControllerUnitTests
    {
        private DotNetMetricsController _controller;

        private Mock<ILogger<DotNetMetricsController>> _mockLogger;

        private Mock<IMetricsRepository<DotNetMetric>> _mockRepository;

        private Fixture _fixture = new Fixture();

        private Random _random = new Random();


        public DotNetControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<DotNetMetricsController>>();
            _mockRepository = new Mock<IMetricsRepository<DotNetMetric>>();
            MapperConfiguration configMapper = new MapperConfiguration(mp =>
                mp.CreateMap<DotNetMetric, DotNetMetricDto>());
            IMapper mapper = configMapper.CreateMapper();
            _controller = new DotNetMetricsController(_mockLogger.Object, _mockRepository.Object, mapper);
        }


        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var returnList = _fixture.Create<List<DotNetMetric>>();

            _mockRepository.Setup(repository => repository.GetAgentMetricByTimePeriod
                (
                    It.IsAny<int>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>()
                ))
                .Returns(returnList);

            var fromTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(500));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(500, 1000));
            var agentId = _random.Next(100);

            var resultGetCpuMetricsTimeInterval = (OkObjectResult)_controller.GetDotNetMetricsFromAgent(agentId, fromTime, toTime);
            var returnListDto = (AllMetricsResponse<DotNetMetricDto>)resultGetCpuMetricsTimeInterval.Value;

            _mockRepository.Verify(repository => repository.GetAgentMetricByTimePeriod
            (
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()
            ), Times.Once());

            Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsTimeInterval);

            for (int i = 0; i < returnList.Count; i++)
            {
                Assert.Equal(returnList[i].Id, returnListDto.Metrics[i].Id);
                Assert.Equal(returnList[i].Time, returnListDto.Metrics[i].Time);
                Assert.Equal(returnList[i].Value, returnListDto.Metrics[i].Value);
            }
        }
    }
}
