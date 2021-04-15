﻿using AutoMapper;
using Core;
using MetricsManager.DAL;
using MetricsManager.DAL.Interfaces;
using MetricsManager.Responses;
using MetricsManager.Responses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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


        /// <summary>
        /// Получает метрики CPU на заданном диапазоне времени от определенного агента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/cpu/agent/1/from/00:00:00/to/23:59:59
        /// , если необходимо указать дату
        ///     GET api/metrics/cpu/agent/1/from/1970-01-01T00:00:00.070Z/to/2022-10-02T05:04:18.070Z
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


        /// <summary>
        /// Получает персентиль метрик CPU на заданном диапазоне времени от определенного агента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/cpu/agent/1/from/00:00:00/to/23:59:59/percentiles/P.95
        /// , если необходимо указать дату
        ///     GET api/metrics/cpu/agent/1/from/1970-01-01T00:00:00.070Z/to/2022-10-02T05:04:18.070Z/percentiles/P.99
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

            int listIndexOfPercentile = metrics.Count * (int)percentile / 100;

            response.Metrics.Add(_mapper.Map<CpuMetricDto>(metrics[listIndexOfPercentile]));

            return Ok(response);
        }


        /// <summary>
        /// Получает метрики CPU на заданном диапазоне времени от всего кластера
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/cpu/cluster/from/00:00:00/to/23:59:59
        /// , если необходимо указать дату
        ///     GET api/metrics/cpu/cluster/from/1970-01-01T00:00:00.070Z/to/2022-10-02T05:04:18.070Z
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метрика времени с 01.01.1970</param>
        /// <param name="toTime">конечная метрика времени с 01.01.1970</param>
        /// <returns>Список метрик, от всего кластера, которые были сохранены в заданном диапазоне времени </returns>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">если передали не правильные параметры</response>  
        /// 
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


        /// <summary>
        /// Получает персентиль метрик CPU на заданном диапазоне времени от всего кластера
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET api/metrics/cpu/cluster/from/00:00:00/to/23:59:59/percentiles/P.95
        /// , если необходимо указать дату
        ///     GET api/metrics/cpu/cluster/from/1970-01-01T00:00:00.070Z/to/2022-10-02T05:04:18.070Z/percentiles/P.99
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
            int listIndexOfPercentile = metrics.Count * (int)percentile / 100;

            response.Metrics.Add(_mapper.Map<CpuMetricDto>(metrics[listIndexOfPercentile]));

            return Ok(response);
        }

    }
}
