using Core;
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
    public class RamControllerUnitTests
    {
        private RamMetricsController _controller;

        private Mock<ILogger<RamMetricsController>> _mockLogger;

        private Mock<IMetricsRepository<RamMetric>> _mockRepository;

        private Fixture _fixture = new Fixture();

        private Random _random = new Random();


        public RamControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<RamMetricsController>>();
            _mockRepository = new Mock<IMetricsRepository<RamMetric>>();
            MapperConfiguration configMapper = new MapperConfiguration(mp =>
                mp.CreateMap<RamMetric, RamMetricDto>());
            IMapper mapper = configMapper.CreateMapper();
            _controller = new RamMetricsController(_mockLogger.Object, _mockRepository.Object, mapper);
        }


        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var returnList = _fixture.Create<List<RamMetric>>();

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

            var resultGetCpuMetricsTimeInterval = (OkObjectResult)_controller.GetRamMetricsFromAgent(agentId, fromTime, toTime);
            var returnListDto = (AllMetricsResponse<RamMetricDto>)resultGetCpuMetricsTimeInterval.Value;

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


        [Fact]
        public void GetMetricsByPercentileFromAgent_ReturnsOk()
        {
            var returnList = _fixture.Create<List<RamMetric>>();

            _mockRepository.Setup(repository => repository.GetAgentMetricByTimePeriod
                (
                    It.IsAny<int>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>()
                ))
                .Returns(returnList);

            var fromTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(500));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(500, 1000));
            var agentId = _random.Next(500);
            Percentile percentile = Percentile.P95;

            var resultGetCpuMetricsTimeInterval = (OkObjectResult)_controller.GetRamMetricsByPercentileFromAgent(agentId, fromTime, toTime, percentile);
            var returnListDto = (AllMetricsResponse<RamMetricDto>)resultGetCpuMetricsTimeInterval.Value;

            _mockRepository.Verify(repository => repository.GetAgentMetricByTimePeriod
            (
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()
            ), Times.Once());

            Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsTimeInterval);
            Assert.IsAssignableFrom<AllMetricsResponse<RamMetricDto>>(returnListDto);
        }


        [Fact]
        public void GetMetricsFromAllCluster_ReturnsOk()
        {
            var returnList = _fixture.Create<List<RamMetric>>();

            _mockRepository.Setup(repository => repository.GetClusterMetricsByTimePeriod
                (
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>()
                ))
                .Returns(returnList);

            var fromTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(500));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(500, 1000));

            var resultGetCpuMetricsTimeInterval = (OkObjectResult)_controller.GetRamMetricsFromAllCluster(fromTime, toTime);
            var returnListDto = (AllMetricsResponse<RamMetricDto>)resultGetCpuMetricsTimeInterval.Value;

            _mockRepository.Verify(repository => repository.GetClusterMetricsByTimePeriod
            (
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


        [Fact]
        public void GetMetricsByPercentileFromAllCluster_ReturnsOk()
        {
            var returnList = _fixture.Create<List<RamMetric>>();

            _mockRepository.Setup(repository => repository.GetClusterMetricsByTimePeriod
                (
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>()
                ))
                .Returns(returnList);

            var fromTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(500));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(_random.Next(500, 1000));
            Percentile percentile = Percentile.P95;

            var resultGetCpuMetricsTimeInterval = (OkObjectResult)_controller.GetRamMetricsByPercentileFromAllCluster(fromTime, toTime, percentile);
            var returnListDto = (AllMetricsResponse<RamMetricDto>)resultGetCpuMetricsTimeInterval.Value;

            _mockRepository.Verify(repository => repository.GetClusterMetricsByTimePeriod
            (
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()
            ), Times.Once());

            Assert.IsAssignableFrom<IActionResult>(resultGetCpuMetricsTimeInterval);
            Assert.IsAssignableFrom<AllMetricsResponse<RamMetricDto>>(returnListDto);
        }
    }
}
