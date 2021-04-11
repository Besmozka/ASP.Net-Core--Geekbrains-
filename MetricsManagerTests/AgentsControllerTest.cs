using MetricsManager.Controllers;
using MetricsManager;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using System.Text;
using Moq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private AgentsController _controller;

        private AgentsList _agentsList;

        private Mock<ILogger<AgentsController>> _mockLogger;

        private Mock<AgentsList> mockAgentsList;

        private int _maxAgentsCount = 1000;
        public AgentsControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<AgentsController>>();
            mockAgentsList = new Mock<AgentsList>();
            _controller = new AgentsController(_mockLogger.Object, mockAgentsList.Object);
        }

        [Fact]
        public void AgentRegister_ReturnsOk()
        {
            //Arrange
            Random random = new Random();
            UriBuilder uriBuilder = new UriBuilder();
            AgentInfo agentInfo = new AgentInfo(random.Next(_maxAgentsCount), uriBuilder.Uri);

            //Act
            var result = _controller.RegisterAgent(agentInfo);

            // Assert
            // ��������� �������� �� ��, ��� ���� ������� ����������
            // ������������� �������� ����� Create ����������� � ������ ����� ������� � ���������
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void EnableAgent_ReturnsOk()
        {
            //Arrange
            Random random = new Random();

            //Act
            var result = _controller.EnableAgentById(random.Next(_maxAgentsCount));

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgent_ReturnsOk()
        {
            //Arrange
            Random random = new Random();

            //Act
            var result = _controller.DisableAgentById(random.Next(_maxAgentsCount));

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void NumberOfAgents_ReturnsOk()
        {
            //Arrange
            Random random = new Random();

            //Act
            var result = _controller.GetNumberOfAgents();

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
