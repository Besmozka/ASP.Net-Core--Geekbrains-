using MetricsAgent;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private IHddMetricsRepository _repository;

        private readonly ILogger<HddMetricsController> _logger;
        public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug("Nlog встроен в HddMetricsController");
        }

        [HttpGet("hdd/left")]
        public IActionResult GetHddSizeLeft()
        {
            _logger.LogInformation($"GetHddSizeLeft - Left:");
            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Get all");
            List<HddMetric> metrics = _repository.GetAll();
            return Ok(metrics);
        }
    }
}
