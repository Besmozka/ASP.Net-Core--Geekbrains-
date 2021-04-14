using System;
using System.Collections.Generic;
using AutoFixture;
using AutoMapper;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MetricsAgentTests
{
    public class RamControllerUnitTests
    {
        private RamMetricsController _controller;


        private Mock<ILogger<RamMetricsController>> _mockLogger;


        private Mock<IRamMetricsRepository> _mockRepository;


        public RamControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<RamMetricsController>>();
            _mockRepository = new Mock<IRamMetricsRepository>();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mp =>
                mp.CreateMap<RamMetric, RamMetricDto>());
            IMapper mapper = mapperConfiguration.CreateMapper();
            _controller = new RamMetricsController(_mockLogger.Object, _mockRepository.Object, mapper);
        }


        [Fact]
        public void GetAvailableSize_ReturnsOk()
        {
            Fixture fixture = new Fixture();
            var returnList = fixture.Create<List<RamMetric>>();

            _mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(returnList);

            var resultGetSizeLeft = (OkObjectResult)_controller.GetRamAvailableSize();
            var returnListDto = (AllMetricsResponse<RamMetricDto>)resultGetSizeLeft.Value;

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
