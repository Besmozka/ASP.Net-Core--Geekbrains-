using AutoMapper;
using EnumsLibrary;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
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

        private readonly IMapper _mapper;
        public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug("Nlog встроен в CpuMetricsController");
            _mapper = mapper;
        }

        [HttpGet("from/{fromTime}/to/{toTime}/")]
        public IActionResult GetCpuMetricsTimeInterval([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetCpuMetricsTimeInterval - From time: {fromTime}; To time: {toTime}");
            CpuMetricDto cpuMetric = new CpuMetricDto();
            cpuMetric.Time = fromTime;
            _repository.Create(_mapper.Map<CpuMetric>(cpuMetric));
            cpuMetric.Time = toTime;
            _repository.Create(_mapper.Map<CpuMetric>(cpuMetric));
            return Ok();
        }

        [HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetCpuMetricsByPercentileTimeInterval([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetCpuMetricsByPercentileTimeInterval - From time: {fromTime}; To time: {toTime}; Percentile: {percentile}");
            CpuMetricDto cpuMetric = new CpuMetricDto();
            cpuMetric.Time = fromTime;
            _repository.Create(_mapper.Map<CpuMetric>(cpuMetric));
            cpuMetric.Time = toTime;
            _repository.Create(_mapper.Map<CpuMetric>(cpuMetric));
            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IList<CpuMetric> metrics = _repository.GetAll();

            var response = new AllMetricsResponse<CpuMetricDto>()
            {
                Metrics = new List<CpuMetricDto>()
            };


            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }

            var dto = new CpuMetricDto { Id = 1, Time = DateTimeOffset.FromUnixTimeSeconds(100), Value = 10 };

            return Ok(response);
        }
    }
}
