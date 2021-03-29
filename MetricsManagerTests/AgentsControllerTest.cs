using MetricsManager.Controllers;
using MetricsManager;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using System.Text;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private AgentsController controller;

        private readonly AgentsList agentsList;

        private int maxAgentsCount = 1000;
        public AgentsControllerUnitTests()
        {
            controller = new AgentsController(agentsList);
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
