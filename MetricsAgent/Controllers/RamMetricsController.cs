using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        [HttpGet("ram/available")]
        public IActionResult GetRamAvailableSize()
        {
            return Ok();
        }
    }
}
