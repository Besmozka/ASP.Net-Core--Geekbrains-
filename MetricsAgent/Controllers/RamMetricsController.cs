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
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private IRamMetricsRepository _repository;

        private readonly ILogger<RamMetricsController> _logger;

        private readonly IMapper _mapper;
        public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug("Nlog встроен в RamMetricsController");
            _mapper = mapper;
        }

        [HttpGet("available")]
        public IActionResult GetRamAvailableSize()
        {
            _logger.LogInformation($"GetRamAvailableSize - Available:");
            List<RamMetric> metrics = _repository.GetByTimePeriod(DateTimeOffset.MinValue, DateTimeOffset.UtcNow);

            var response = new AllMetricsResponse<RamMetricDto>()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }
            return Ok(response);
        }
    }
}
