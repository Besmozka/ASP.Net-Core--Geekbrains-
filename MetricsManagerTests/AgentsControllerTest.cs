using MetricsManager.Controllers;
using MetricsManager;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using System.Text;
using Moq;
using Microsoft.Extensions.Logging;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private AgentsController controller;

        private AgentsList agentsList;

        private Mock<ILogger<AgentsController>> mockLogger;

        private int maxAgentsCount = 1000;
        public AgentsControllerUnitTests()
        {
            mockLogger = new Mock<ILogger<AgentsController>>();
            controller = new AgentsController(mockLogger.Object, agentsList);
        }

        [Fact]
        public void AgentRegister_ReturnsOk()
        {
            //Arrange
            Random random = new Random();
            UriBuilder uriBuilder = new UriBuilder();
            AgentInfo agentInfo = new AgentInfo(random.Next(maxAgentsCount), uriBuilder.Uri);

            //Act
            var result = controller.RegisterAgent(agentInfo);

            // Assert
            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void EnableAgent_ReturnsOk()
        {
            //Arrange
            Random random = new Random();

            //Act
            var result = controller.EnableAgentById(random.Next(maxAgentsCount));

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgent_ReturnsOk()
        {
            //Arrange
            Random random = new Random();

            //Act
            var result = controller.DisableAgentById(random.Next(maxAgentsCount));

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void NumberOfAgents_ReturnsOk()
        {
            //Arrange
            Random random = new Random();

            //Act
            var result = controller.GetNumberOfAgents();

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
