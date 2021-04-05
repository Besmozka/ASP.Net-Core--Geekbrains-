using EnumsLibrary;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MetricsAgent.Controllers
{

    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private ICpuMetricsRepository _repository;

        private readonly ILogger<CpuMetricsController> _logger;
        public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository repository)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug("Nlog встроен в CpuMetricsController");
        }

        [HttpGet("from/{fromTime}/to/{toTime}/")]
        public IActionResult GetCpuMetricsTimeInterval([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetCpuMetricsTimeInterval - From time: {fromTime}; To time: {toTime}");
            List<CpuMetric> metrics = _repository.GetByTimePeriod(fromTime, toTime);

            return Ok(metrics);
        }

        [HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetCpuMetricsByPercentileTimeInterval([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetCpuMetricsByPercentileTimeInterval - From time: {fromTime}; To time: {toTime}; Percentile: {percentile}");
            List<CpuMetric> metrics = _repository.GetByTimePeriod(fromTime, toTime);

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Get all");
            List<CpuMetric> metrics = _repository.GetAll();
            return Ok(metrics);
        }
    }
}
