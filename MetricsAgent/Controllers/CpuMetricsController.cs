using EnumsLibrary;
using Microsoft.AspNetCore.Mvc;
using System;


namespace MetricsAgent.Controllers
{

    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        [HttpGet("from/{fromTime}/to/{toTime}/")]
        public IActionResult GetCpuMetricsTimeInterval([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }

        [HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetCpuMetricsByPercentileTimeInterval([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime,
            [FromRoute] Percentile percentile)
        {
            return Ok();
        }
    }
}
