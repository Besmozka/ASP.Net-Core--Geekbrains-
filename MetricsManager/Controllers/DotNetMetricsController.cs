using AutoMapper;
using MetricsManager.DAL;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repositories;
using MetricsManager.Responses;
using MetricsManager.Responses.Models;
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
        private readonly ILogger<DotNetMetricsController> _logger;

        private readonly IMetricsRepository<DotNetMetric> _repository;

        private readonly IMapper _mapper;


        public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IMetricsRepository<DotNetMetric> repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в DotNetMetricsController");
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet("errors-count/agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetDotNetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetDotNetMetricsFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime}");

            List<DotNetMetric> metrics = _repository.GetAgentMetricByTimePeriod(agentId, fromTime, toTime);

            var response = new AllMetricsResponse<DotNetMetricDto>
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
