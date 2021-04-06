using AutoMapper;
using MetricsAgent;
using MetricsAgent.DAL;
using MetricsManager.Controllers;
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

        private Mock<IMapper> _mockMapper;
        public HddControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<HddMetricsController>>();
            _mockRepository = new Mock<IHddMetricsRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new HddMetricsController(_mockLogger.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetSizeLeft_ReturnsOk()
        {
            var result = _controller.GetHddSizeLeft();

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Call_GetAll_From_Controller()
        {
            _mockRepository.Setup(repository => repository.GetAll()).Returns(new List<RamMetric>()); ;

            var resultGetAll = _controller.GetAll();

            _mockRepository.Verify(repository => repository.GetAll(), Times.Once());
            _ = Assert.IsAssignableFrom<IActionResult>(resultGetAll);
        }
    }
}
