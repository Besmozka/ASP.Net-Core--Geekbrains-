using AutoMapper;
using MetricsAgent;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private INetworkMetricsRepository _repository;

        private readonly ILogger<NetworkMetricsController> _logger;

        private readonly IMapper _mapper;
        public NetworkMetricsController(ILogger<NetworkMetricsController> logger, INetworkMetricsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug("Nlog встроен в NetworkMetricsController");
            _mapper = mapper;
        }

        [HttpGet("from/{fromTime}/to/{toTime}/")]
        public IActionResult GetNetworkMetricsTimeInterval([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetNetworkMetricsTimeInterval - From time: {fromTime}; To time: {toTime}");

            List<NetworkMetric> metrics = _repository.GetByTimePeriod(fromTime, toTime);

            var response = new AllMetricsResponse<NetworkMetricDto>()
            {
                Metrics = new List<NetworkMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
            }
            return Ok(response);
        }
    }
}
