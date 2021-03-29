using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        [HttpGet("errors-count/agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetDotNetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }
    }
}
