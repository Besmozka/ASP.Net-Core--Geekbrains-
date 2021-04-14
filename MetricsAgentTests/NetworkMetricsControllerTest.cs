using AutoMapper;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using AutoFixture;
using MetricsAgent.Responses;
using Xunit;

namespace MetricsAgentTests
{
    public class NetworkControllerUnitTests
    {
        private NetworkMetricsController _controller;


        private Mock<ILogger<NetworkMetricsController>> _mockLogger;


        private Mock<INetworkMetricsRepository> _mockRepository;


        public NetworkControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<NetworkMetricsController>>();
            _mockRepository = new Mock<INetworkMetricsRepository>();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mp =>
                mp.CreateMap<NetworkMetric, NetworkMetricDto>());
            IMapper mapper = mapperConfiguration.CreateMapper();
            _controller = new NetworkMetricsController(_mockLogger.Object, _mockRepository.Object, mapper);
        }


        [Fact]
        public void GetMetricsTimeInterval_ReturnsOk()
        {
            Fixture fixture = new Fixture();
            var returnList = fixture.Create<List<NetworkMetric>>();

            _mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(returnList);

            Random random = new Random();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(500));
            var toTime = DateTimeOffset.FromUnixTimeSeconds(random.Next(500, 1000));

            var resultGetSizeLeft = (OkObjectResult)_controller.GetNetworkMetricsTimeInterval(fromTime,toTime);
            var returnListDto = (AllMetricsResponse<NetworkMetricDto>)resultGetSizeLeft.Value;

            _mockRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()),
                Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetSizeLeft);

            for (int i = 0; i < returnList.Count; i++)
            {
                Assert.Equal(returnList[i].Id, returnListDto.Metrics[i].Id);
                Assert.Equal(returnList[i].Time, returnListDto.Metrics[i].Time);
                Assert.Equal(returnList[i].Value, returnListDto.Metrics[i].Value);
            }
        }
    }
}
