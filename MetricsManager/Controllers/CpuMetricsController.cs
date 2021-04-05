using EnumsLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;

        public CpuMetricsController(ILogger<CpuMetricsController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetCpuMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation($"GetCpuMetricsFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime}");
            return Ok();
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetCpuMetricsByPercentileFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime, 
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetCpuMetricsByPercentileFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime};" +
                $" Percentile: {percentile}");
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetCpuMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation($"GetCpuMetricsFromAllCluster - Agent ID: From all cluster; From time: {fromTime}; To time: {toTime}");
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetCpuMetricsByPercentileFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime,
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetCpuMetricsByPercentileFromAllCluster - Agent ID: From all cluster; From time: {fromTime};" +
                $" To time: {toTime}; Percentile: {percentile}");
            return Ok();
        }

    }
}
