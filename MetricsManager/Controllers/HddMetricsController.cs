using EnumsLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;

        public HddMetricsController(ILogger<HddMetricsController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в HddMetricsController");
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetHddMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation($"GetHddMetricsFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime}");
            return Ok();
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetHddMetricsByPercentileFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime, 
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetHddMetricsByPercentileFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime};" +
                $" Percentile: {percentile}");
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetHddMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation($"GetHddMetricsFromAllCluster - Agent ID: From all cluster; From time: {fromTime}; To time: {toTime}");
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetHddMetricsByPercentileFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime,
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetHddMetricsByPercentileFromAllCluster - Agent ID: From all cluster; From time: {fromTime}; " +
                $"To time: {toTime}; Percentile: {percentile}");
            return Ok();
        }
    }
}
