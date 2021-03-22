using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetNetworkMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetNetWorkMetricsByPercentileFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetNetworkMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetNetworkMetricsByPercentileFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime,
            [FromRoute] Percentile percentile)
        {
            return Ok();
        }
    }
}
