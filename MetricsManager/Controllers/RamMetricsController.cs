using AutoMapper;
using Core;
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
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;

        private readonly IMetricsRepository<RamMetric> _repository;

        private readonly IMapper _mapper;


        public RamMetricsController(ILogger<RamMetricsController> logger, IMetricsRepository<RamMetric> repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug("Nlog встроен в RamMetricsController");
            _repository = repository;
            _mapper = mapper;
        }


        /// <summary>
        /// Получает метрики RAM на заданном диапазоне времени от определенного агента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/ram/agent/1/from/00:00:00/to/23:59:59
        /// , если необходимо указать дату
        ///     GET api/metrics/ram/agent/1/from/1970-01-01T00:00:00.070Z/to/2022-10-02T05:04:18.070Z
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


        /// <summary>
        /// Получает персентиль метрик RAM на заданном диапазоне времени от определенного агента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/ram/agent/1/from/00:00:00/to/23:59:59/percentiles/P.95
        /// , если необходимо указать дату
        ///     GET api/metrics/ram/agent/1/from/1970-01-01T00:00:00.070Z/to/2022-10-02T05:04:18.070Z/percentiles/P.99
        ///
        /// </remarks>
        /// <param name="agentId">Id клиента</param>
        /// <param name="fromTime">начальная метрика времени с 01.01.1970</param>
        /// <param name="toTime">конечная метрика времени с 01.01.1970</param>
        /// <param name="percentile">персентиль из списка: Median, P75, P90, P95, P99</param>
        /// <returns>Список метрик, от определенного агента, которые были сохранены в заданном диапазоне времени </returns>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">если передали не правильные параметры</response>  
        /// 
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


        /// <summary>
        /// Получает метрики RAM на заданном диапазоне времени от всего кластера
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/ram/cluster/from/00:00:00/to/23:59:59
        /// , если необходимо указать дату
        ///     GET api/metrics/ram/cluster/from/1970-01-01T00:00:00.070Z/to/2022-10-02T05:04:18.070Z
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метрика времени с 01.01.1970</param>
        /// <param name="toTime">конечная метрика времени с 01.01.1970</param>
        /// <returns>Список метрик, от всего кластера, которые были сохранены в заданном диапазоне времени </returns>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">если передали не правильные параметры</response>  
        /// 
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


        /// <summary>
        /// Получает персентиль метрик RAM на заданном диапазоне времени от всего кластера
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/ram/cluster/from/00:00:00/to/23:59:59/percentiles/P.95
        /// , если необходимо указать дату
        ///     GET api/metrics/ram/cluster/from/1970-01-01T00:00:00.070Z/to/2022-10-02T05:04:18.070Z/percentiles/P.99
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метрика времени с 01.01.1970</param>
        /// <param name="toTime">конечная метрика времени с 01.01.1970</param>
        /// <param name="percentile">персентиль из списка: Median, P75, P90, P95, P99</param>
        /// <returns>Список метрик, от всего кластера, которые были сохранены в заданном диапазоне времени </returns>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">если передали не правильные параметры</response>  
        /// 
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
