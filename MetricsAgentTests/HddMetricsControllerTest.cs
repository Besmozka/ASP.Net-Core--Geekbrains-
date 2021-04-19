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
    public class HddControllerUnitTests
    {
        private HddMetricsController _controller;


        private Mock<ILogger<HddMetricsController>> _mockLogger;


        private Mock<IHddMetricsRepository> _mockRepository;


        public HddControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<HddMetricsController>>();
            _mockRepository = new Mock<IHddMetricsRepository>();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mp =>
                mp.CreateMap<HddMetric, HddMetricDto>());
            IMapper mapper = mapperConfiguration.CreateMapper();
            _controller = new HddMetricsController(_mockLogger.Object, _mockRepository.Object, mapper);
        }


        [Fact]
        public void GetSizeLeft_ReturnsOk()
        {
            Fixture fixture = new Fixture();
            var returnList = fixture.Create<List<HddMetric>>();

            _mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(returnList);

            var resultGetSizeLeft = (OkObjectResult)_controller.GetHddSizeLeft();
            var returnListDto = (AllMetricsResponse<HddMetricDto>)resultGetSizeLeft.Value;

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
