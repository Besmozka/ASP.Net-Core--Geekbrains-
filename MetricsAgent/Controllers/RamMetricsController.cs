using MetricsAgent;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private IRamMetricsRepository _repository;

        private readonly ILogger<RamMetricsController> _logger;
        public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository repository)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug("Nlog встроен в RamMetricsController");
        }

        [HttpGet("ram/available")]
        public IActionResult GetRamAvailableSize()
        {
            _logger.LogInformation($"GetRamAvailableSize - Available:");
            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Get all");
            List<RamMetric> metrics = _repository.GetAll();
            return Ok(metrics);
        }
    }
}
