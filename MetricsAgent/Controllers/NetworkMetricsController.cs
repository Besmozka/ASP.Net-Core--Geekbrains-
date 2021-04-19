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


        /// <summary>
        /// Получает метрики Network на заданном диапазоне времени
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET networkmetrics/from/00:00:00/to/23:59:59
        /// , если необходимо указать дату
        ///     GET networkmetrics/from/1970-01-01T00:00:00.070Z/to/2022-10-02T05:04:18.070Z
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метрика времени с 01.01.1970</param>
        /// <param name="toTime">конечная метрика времени с 01.01.1970</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">если передали не правильные параметры</response>  
        /// 
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
