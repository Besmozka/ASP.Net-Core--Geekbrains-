using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL;
using AutoMapper;
using MetricsManager.DAL.Interfaces;
using System.Collections.Generic;
using MetricsManager.Responses;
using MetricsManager.Responses.Models;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;

        private readonly IMetricsRepository<CpuMetric> _repository;

        private readonly IMapper _mapper;

        public CpuMetricsController(ILogger<CpuMetricsController> logger, IMetricsRepository<CpuMetric> repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetCpuMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetCpuMetricsFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime}");
            List<CpuMetric> metrics = _repository.GetAgentMetricByTimePeriod(agentId, fromTime, toTime);

            var response = new AllMetricsResponse<CpuMetricDto>
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetCpuMetricsByPercentileFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime, [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetCpuMetricsByPercentileFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime};" +
                $" Percentile: {percentile}");
            List<CpuMetric> metrics = _repository.GetAgentMetricByTimePeriod(agentId, fromTime, toTime);

            var response = new AllMetricsResponse<CpuMetricDto>
            {
                Metrics = new List<CpuMetricDto>()
            };
            response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric)); // вычислить персентиль и передать в ответ
            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetCpuMetricsFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetCpuMetricsFromAllCluster - Agent ID: From all cluster; From time: {fromTime}; To time: {toTime}");

            List<CpuMetric> metrics = _repository.GetClusterMetricsByTimePeriod(fromTime, toTime);

            var response = new AllMetricsResponse<CpuMetricDto>
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetCpuMetricsByPercentileFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetCpuMetricsByPercentileFromAllCluster - Agent ID: From all cluster; From time: {fromTime};" +
                $" To time: {toTime}; Percentile: {percentile}");

            List<CpuMetric> metrics = _repository.GetClusterMetricsByTimePeriod(fromTime, toTime);

            var response = new AllMetricsResponse<CpuMetricDto>
            {
                Metrics = new List<CpuMetricDto>()
            };
            response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric)); // вычислить персентиль и передать в ответ
            return Ok(response);
        }

    }
}
