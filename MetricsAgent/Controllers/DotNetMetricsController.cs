using MetricsAgent;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private DotNetRepository _repository;

        private readonly ILogger<DotNetMetricsController> _logger;
        public DotNetMetricsController(ILogger<DotNetMetricsController> logger)
        {
            _logger = logger;
            _logger.LogDebug("Nlog встроен в DotNetMetricsController");
        }
        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetDotNetErrorsTimeInterval([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetDotNetErrorsTimeInterval - From time: {fromTime}; To time: {toTime}");
            DotNetMetric dotNetMetric = new DotNetMetric();
            dotNetMetric.Time = fromTime;
            _repository.Create(dotNetMetric);
            dotNetMetric.Time = toTime;
            _repository.Create(dotNetMetric);
            return Ok();
        }
    }
}
