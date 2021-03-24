using MetricsAgent.Enums;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        [HttpGet("hdd/left")]
        public IActionResult GetHddSizeLeft()
        {
            return Ok();
        }
    }
}
