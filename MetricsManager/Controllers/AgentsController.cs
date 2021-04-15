using MetricsManager.DAL;
using MetricsManager.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace MetricsManager.Controllers
{
    [Route("api/agents")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ILogger<AgentsController> _logger;

        private readonly IAgentsRepository _repository;

        private readonly AgentsList _agents;


        public AgentsController(ILogger<AgentsController> logger, IAgentsRepository repository, AgentsList agents)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
            _repository = repository;
            _agents = agents;
        }


        /// <summary>
        /// Регистрирует нового агента
        /// </summary>
        /// <remarks>
        /// Данные передаются в теле запроса
        /// Пример запроса:
        /// 
        ///     POST api/agents
        /// Body.json
        /// {
        ///     AgentId = 1,
        ///     AgentAddress = http://localhost:5000
        /// }
        /// 
        /// </remarks>
        /// <param name="AgentId">Id клиента</param>
        /// <param name="AgentAddress">URL адрес клиента</param>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">если передали не правильные параметры</response>  
        /// 
        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            GetAgentsListFromRepository();
            _logger.LogInformation($"RegisterAgent - Agent Info: {agentInfo}");
            if (_agents.Agents.Exists(item => item.AgentAddress == agentInfo.AgentAddress))
            {
                return BadRequest($"Агент с таким адресом({agentInfo.AgentAddress}) существует");
            }
            _repository.Create(agentInfo);
            return Ok();
        }


        /// <summary>
        /// Удаляет агента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     DELETE api/delete/"http://localhost:5000"
        ///  
        /// </remarks>
        /// <param name="AgentAddress">URL адрес клиента</param>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">если передали не правильные параметры</response>  
        ///
        [HttpDelete("delete/{agentAddress}")]
        public IActionResult DeleteAgent([FromRoute] string agentAddress)
        {
            _logger.LogInformation($"Delete agent - Agent Address: {agentAddress}");
            _repository.Delete(agentAddress);
            return Ok();
        }


        /// <summary>
        /// Получает количество зарегистрированных клиентов
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST api/numberAgents
        /// 
        /// </remarks>
        /// <returns>Возвращается количество зарегистрированных клиентов</returns>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">если передали не правильные параметры</response>  
        ///
        [HttpGet("numberAgents")]
        public IActionResult GetNumberOfAgents()
        {
            var numberOfAgents = _repository.GetAgents().Count;
            _logger.LogInformation($"GetNumberOfAgents - {numberOfAgents}");
            return Ok($"Количество агентов: {numberOfAgents}");
        }

        private void GetAgentsListFromRepository()
        {
            _agents.Agents = _repository.GetAgents();
        }
    }
}
