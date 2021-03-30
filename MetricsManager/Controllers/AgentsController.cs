using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Controllers
{
    [Route("api/agents")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly AgentsList agentsList = new AgentsList();

        private readonly ILogger<AgentsController> _logger;

        public AgentsController(ILogger<AgentsController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
        }

        public AgentsController(AgentsList agentsList)
        {
            this.agentsList = agentsList;
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            agentsList.agents.Add(agentInfo);
            _logger.LogInformation($"Agent Info: {agentInfo}");
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }

        [HttpGet("numberAgents")]
        public IActionResult GetNumberOfAgents()
        {
            _logger.LogInformation("Привет!");
            return Ok($"Количество агентов: {agentsList.agents.Count}");
        }
    }
}
