using MetricsManager.Controllers;
using MetricsManager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;
using System.Text;
using AutoFixture;
using Moq;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL;
using MetricsManager.DAL.Interfaces;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private AgentsController _controller;

        private Mock<ILogger<AgentsController>> _mockLogger;

        private Mock<IAgentsRepository> _mockRepository;

        private int _maxAgentsCount = 100;

        private readonly Random _random = new Random();

        private UriBuilder _uriBuilder = new UriBuilder();

        private Fixture _fixture = new Fixture();


        public AgentsControllerUnitTests()
        {
            var agentsList = _fixture.Create<AgentsList>();
            _mockLogger = new Mock<ILogger<AgentsController>>();
            _mockRepository = new Mock<IAgentsRepository>();
            _controller = new AgentsController(_mockLogger.Object, _mockRepository.Object, agentsList);
        }


        [Fact]
        public void AgentRegister_ReturnsOk()
        {
            var returnList = _fixture.Create<List<AgentInfo>>();
            AgentInfo agentInfo = new AgentInfo
            {
                AgentId = _random.Next(_maxAgentsCount), 
                AgentAddress = _uriBuilder.Uri

            };

            _mockRepository.Setup(repository => repository.GetAgents()).Returns(returnList);
            _mockRepository.Setup(repository =>repository.Create(It.IsAny<AgentInfo>())).Verifiable();
            var resultAgentRegister = _controller.RegisterAgent(agentInfo);

            _mockRepository.Verify(repository => repository.GetAgents(), Times.Once);
            _mockRepository.Verify(repository => repository.Create(It.IsAny<AgentInfo>()), Times.Once);

            Assert.IsAssignableFrom<IActionResult>(resultAgentRegister);
        }


        [Fact]
        public void DeleteAgent_ReturnsOk()
        {
            _mockRepository.Setup(repository =>repository.Delete(It.IsAny<String>())).Verifiable();

            var resultDeleteAgent = _controller.DeleteAgent(_uriBuilder.Uri.ToString());

            _mockRepository.Verify(repository => repository.Delete(It.IsAny<String>()), Times.Once);

            _ = Assert.IsAssignableFrom<IActionResult>(resultDeleteAgent);
        }


        [Fact]
        public void GetNumberOfAgents_ReturnsOk()
        {
            var listAgents = _fixture.Create<List<AgentInfo>>();

            _mockRepository.Setup(repository => repository.GetAgents()).Returns(listAgents);
            var result = (OkObjectResult)_controller.GetNumberOfAgents();

            _mockRepository.Verify(repository => repository.GetAgents(), Times.Once());

            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.Equal($"Количество агентов: {listAgents.Count}", result.Value);
        }
    }
}
