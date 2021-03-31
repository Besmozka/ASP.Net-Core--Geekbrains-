using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        public RamMetricsController(ILogger<RamMetricsController> logger)
        {
            _logger = logger;
            _logger.LogDebug("Nlog встроен в RamMetricsController");
        }

        [HttpGet("ram/available")]
        public IActionResult GetRamAvailableSize()
        {
            _logger.LogInformation($"GetRamAvailableSize - Available:");
            return Ok();
        }
    }
}
