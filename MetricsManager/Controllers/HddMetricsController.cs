using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL;
using System.Collections.Generic;
using AutoMapper;
using MetricsManager.DAL.Repositories;
using MetricsManager.Responses;
using MetricsManager.Responses.Models;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;

        private readonly HddMetricsRepository _repository;

        private readonly IMapper _mapper;

        public HddMetricsController(ILogger<HddMetricsController> logger, HddMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в HddMetricsController");
            _repository = repository;
            _mapper = mapper;                                                                       
        }       
                
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetHddMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetHddMetricsFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime}");

            List<HddMetric> metrics = _repository.GetAgentMetricByTimePeriod(agentId, fromTime, toTime);

            var response = new AllMetricsResponse<HddMetricDto>
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetHddMetricsByPercentileFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime, 
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetHddMetricsByPercentileFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime};" +
                $" Percentile: {percentile}");

            List<HddMetric> metrics = _repository.GetAgentMetricByTimePeriod(agentId, fromTime, toTime);

            var response = new AllMetricsResponse<HddMetricDto>
            {
                Metrics = new List<HddMetricDto>()
            };
            response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetHddMetricsFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetHddMetricsFromAllCluster - Agent ID: From all cluster; From time: {fromTime}; To time: {toTime}");

            List<HddMetric> metrics = _repository.GetClusterMetricsByTimePeriod(fromTime, toTime);

            var response = new AllMetricsResponse<HddMetricDto>
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            }
            return Ok(response);
            return Ok();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetHddMetricsByPercentileFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetHddMetricsByPercentileFromAllCluster - Agent ID: From all cluster; From time: {fromTime}; " +
                $"To time: {toTime}; Percentile: {percentile}");

            List<HddMetric> metrics = _repository.GetClusterMetricsByTimePeriod(fromTime, toTime);

            var response = new AllMetricsResponse<HddMetricDto>
            {
                Metrics = new List<HddMetricDto>()
            };
            response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            return Ok(response);
        }
    }
}
