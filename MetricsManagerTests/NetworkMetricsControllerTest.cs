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
    public class NetworkControllerUnitTests
    {
        private NetworkMetricsController _controller;

        private Mock<ILogger<NetworkMetricsController>> _mockLogger;

        private Mock<IMetricsRepository<NetworkMetric>> _mockRepository;

        private Fixture _fixture = new Fixture();

        private Random _random = new Random();


        public NetworkControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<NetworkMetricsController>>();
            _mockRepository = new Mock<IMetricsRepository<NetworkMetric>>();
            MapperConfiguration configMapper = new MapperConfiguration(mp =>
                mp.CreateMap<NetworkMetric, NetworkMetricDto>());
            IMapper mapper = configMapper.CreateMapper();
            _controller = new NetworkMetricsController(_mockLogger.Object, _mockRepository.Object, mapper);
        }


        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var returnList = _fixture.Create<List<NetworkMetric>>();

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

            var resultGetCpuMetricsTimeInterval = (OkObjectResult)_controller.GetNetworkMetricsFromAgent(agentId, fromTime, toTime);
            var returnListDto = (AllMetricsResponse<NetworkMetricDto>)resultGetCpuMetricsTimeInterval.Value;

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
