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
    public class RamControllerUnitTests
    {
        private RamMetricsController _controller;

        private Mock<ILogger<RamMetricsController>> _mockLogger;

        private Mock<IRamMetricsRepository> _mockRepository;

        private Mock<IMapper> _mockMapper;
        public RamControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<RamMetricsController>>();
            _mockRepository = new Mock<IRamMetricsRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new RamMetricsController(_mockLogger.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetAvailableSize_ReturnsOk()
        {
            var result = _controller.GetRamAvailableSize();

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
