using AutoMapper;
using Core;
using MetricsManager.DAL;
using MetricsManager.DAL.Repositories;
using MetricsManager.Responses;
using MetricsManager.Responses.Models;
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
        private readonly ILogger<RamMetricsController> _logger;

        private readonly RamMetricsRepository _repository;

        private readonly IMapper _mapper;
        public RamMetricsController(ILogger<RamMetricsController> logger, RamMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug("Nlog встроен в RamMetricsController");
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetRamMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetRamMetricsFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime}");

            List<RamMetric> metrics = _repository.GetAgentMetricByTimePeriod(agentId, fromTime, toTime);

            var response = new AllMetricsResponse<RamMetricDto>
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetRamMetricsByPercentileFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetRamMetricsByPercentileFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime};" +
                $" Percentile: {percentile}");

            List<RamMetric> metrics = _repository.GetAgentMetricByTimePeriod(agentId, fromTime, toTime);

            var response = new AllMetricsResponse<RamMetricDto>
            {
                Metrics = new List<RamMetricDto>()
            };

            int listIndexOfPercentile = metrics.Count * (int)percentile / 100;

            response.Metrics.Add(_mapper.Map<RamMetricDto>(metrics[listIndexOfPercentile]));

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetRamMetricsFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetRamMetricsFromAllCluster - Agent ID: From all cluster; From time: {fromTime}; To time: {toTime}");

            List<RamMetric> metrics = _repository.GetClusterMetricsByTimePeriod(fromTime, toTime);

            var response = new AllMetricsResponse<RamMetricDto>
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetRamMetricsByPercentileFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetRamMetricsByPercentileFromAllCluster - Agent ID: From all cluster; From time: {fromTime};" +
                $" To time: {toTime}; Percentile: {percentile}");

            List<RamMetric> metrics = _repository.GetClusterMetricsByTimePeriod(fromTime, toTime);

            var response = new AllMetricsResponse<RamMetricDto>
            {
                Metrics = new List<RamMetricDto>()
            };

            int listIndexOfPercentile = metrics.Count * (int)percentile / 100;

            response.Metrics.Add(_mapper.Map<RamMetricDto>(metrics[listIndexOfPercentile]));

            return Ok(response);
        }
    }
}
