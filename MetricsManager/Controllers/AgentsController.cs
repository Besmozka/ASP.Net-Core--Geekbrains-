using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Controllers
{
    [Route("api/agents")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly AgentsList _agentsList = new AgentsList();

        private readonly ILogger<AgentsController> _logger;

        public AgentsController(ILogger<AgentsController> logger, AgentsList agentsList)
        {
            _agentsList = agentsList;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            _agentsList.agents.Add(agentInfo);
            _logger.LogInformation($"RegisterAgent - Agent Info: {agentInfo}");
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation($"EnableAgentById - Agent ID: {agentId}");
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation($"DisableAgentById - Agent ID: {agentId}");
            return Ok();
        }

        [HttpGet("numberAgents")]
        public IActionResult GetNumberOfAgents()
        {
            _logger.LogInformation($"GetNumberOfAgents - {_agentsList.agents.Count}");
            return Ok($"Количество агентов: {_agentsList.agents.Count}");
        }
    }
}
