using AutoMapper;
using MetricsAgent;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
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

        private readonly IMapper _mapper;
        public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IDotNetMetricsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug("Nlog встроен в DotNetMetricsController");
            _mapper = mapper;
        }
        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetDotNetErrorsTimeInterval([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetDotNetErrorsTimeInterval - From time: {fromTime}; To time: {toTime}");
            DotNetMetricDto dotNetMetric = new DotNetMetricDto();
            dotNetMetric.Time = fromTime;
            _repository.Create(_mapper.Map<DotNetMetric>(dotNetMetric));
            dotNetMetric.Time = toTime;
            _repository.Create(_mapper.Map<DotNetMetric>(dotNetMetric));
            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IList<DotNetMetric> metrics = _repository.GetAll();

            var response = new AllMetricsResponse<DotNetMetricDto>()
            {
                Metrics = new List<DotNetMetricDto>()
            };


            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<DotNetMetricDto>(metric));
            }

            return Ok(response);
        }
    }
}
