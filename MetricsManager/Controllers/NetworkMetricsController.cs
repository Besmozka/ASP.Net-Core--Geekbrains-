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
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;

        private readonly IMetricsRepository<NetworkMetric> _repository;

        private readonly IMapper _mapper;


        public NetworkMetricsController(ILogger<NetworkMetricsController> logger, IMetricsRepository<NetworkMetric> repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug("Nlog встроен в NetworkMetricsController");
            _repository = repository;
            _mapper = mapper;
        }


        /// <summary>
        /// Получает метрики Network на заданном диапазоне времени от определенного агента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/network/agent/1/from/00:00:00/to/23:59:59
        /// , если необходимо указать дату
        ///     GET api/metrics/network/agent/1/from/1970-01-01T00:00:00.070Z/to/2022-10-02T05:04:18.070Z
        ///
        /// </remarks>
        /// <param name="agentId">Id клиента</param>
        /// <param name="fromTime">начальная метрика времени с 01.01.1970</param>
        /// <param name="toTime">конечная метрика времени с 01.01.1970</param>
        /// <returns>Список метрик, от определенного агента, которые были сохранены в заданном диапазоне времени </returns>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">если передали не правильные параметры</response>  
        /// 
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetNetworkMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetNetworkMetricsFromAgent - Agent ID: {agentId}; From time: {fromTime}; To time: {toTime}");

            List<NetworkMetric> metrics = _repository.GetAgentMetricByTimePeriod(agentId, fromTime, toTime);

            var response = new AllMetricsResponse<NetworkMetricDto>
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
