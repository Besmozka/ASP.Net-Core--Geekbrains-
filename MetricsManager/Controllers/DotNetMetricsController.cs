using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL;
using System.Collections.Generic;
using MetricsManager.DAL.Repositories;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsController> _logger;

        private readonly DotNetMetricsRepository _repository;
        public DotNetMetricsController(ILogger<DotNetMetricsController> logger, DotNetMetricsRepository repository)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в DotNetMetricsController");
            _repository = repository;
        }
           
        [HttpGet("errors-count/agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetDotNetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetDotNetMetricsFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime}");
            List<DotNetMetric> metrics = _repository.GetAgentMetricByTimePeriod(agentId, fromTime, toTime);
            return Ok();
        }
    }
}
