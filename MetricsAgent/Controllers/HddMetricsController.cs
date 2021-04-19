using AutoMapper;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private IHddMetricsRepository _repository;

        private readonly ILogger<HddMetricsController> _logger;

        private readonly IMapper _mapper;
        public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug("Nlog встроен в HddMetricsController");
            _mapper = mapper;
        }

        [HttpGet("left")]
        public IActionResult GetHddSizeLeft()
        {
            _logger.LogInformation($"GetHddSizeLeft - Left:");
            List<HddMetric> metrics = _repository.GetByTimePeriod(DateTimeOffset.MinValue, DateTimeOffset.UtcNow);

            var response = new AllMetricsResponse<HddMetricDto>()
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            }
            return Ok(response);
        }
    }
}
