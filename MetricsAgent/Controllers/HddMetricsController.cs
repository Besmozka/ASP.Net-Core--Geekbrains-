using Microsoft.AspNetCore.Mvc;

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
