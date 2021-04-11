using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL;
using System.Collections.Generic;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;
        public NetworkMetricsController(ILogger<NetworkMetricsController> logger)
        {
            _logger = logger;
            _logger.LogDebug("Nlog встроен в NetworkMetricsController");
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetNetworkMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation($"GetNetworkMetricsFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime}");
            return Ok();
        }
    }
}
