using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/agents")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly AgentsList agentsList = new AgentsList();

        public AgentsController(AgentsList agentsList)
        {
            this.agentsList = agentsList;
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            agentsList.agents.Add(agentInfo);
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
            return Ok($"Количество агентов: {agentsList.agents.Count}");
        }
    }
}
