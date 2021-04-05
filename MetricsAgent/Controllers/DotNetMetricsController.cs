using MetricsAgent;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private IDotNetMetricsRepository _repository;

        private readonly ILogger<DotNetMetricsController> _logger;
        public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IDotNetMetricsRepository repository)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug("Nlog встроен в DotNetMetricsController");
        }
        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetDotNetErrorsTimeInterval([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetDotNetErrorsTimeInterval - From time: {fromTime}; To time: {toTime}");
            List<DotNetMetric> metrics = _repository.GetByTimePeriod(fromTime, toTime);
            return Ok(metrics);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Get all");
            List<DotNetMetric> metrics = _repository.GetAll();
            return Ok(metrics);
        }
    }
}
