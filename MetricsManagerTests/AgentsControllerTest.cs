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

        private Mock<AgentsList> mock;

        private ILogger<AgentsController> _logger;

        private int maxAgentsCount = 1000;
        public AgentsControllerUnitTests()
        {
            mock = new Mock<AgentsList>();
            controller = new AgentsController(_logger, mock.Object);
        }

        [Fact]
        public void AgentRegister_ReturnsOk()
        {
            //Arrange
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            mock.Setup(agentList => agentList.agents.Add(It.IsAny<AgentInfo>())).Verifiable();
            Random random = new Random();
            UriBuilder uriBuilder = new UriBuilder();
            AgentInfo agentInfo = new AgentInfo(random.Next(maxAgentsCount), uriBuilder.Uri);

            //Act
            var result = controller.RegisterAgent(agentInfo);

            // Assert
            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            mock.Verify(agentList => agentList.agents.Add(It.IsAny<AgentInfo>()), Times.AtMostOnce());
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
