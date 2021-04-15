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


        /// <summary>
        /// Получает метрики RAM на заданном диапазоне времени
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET rammetrics/from/00:00:00/to/23:59:59
        /// , если необходимо указать дату
        ///     GET rammetrics/from/1970-01-01T00:00:00.070Z/to/2022-10-02T05:04:18.070Z
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метрика времени с 01.01.1970</param>
        /// <param name="toTime">конечная метрика времени с 01.01.1970</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">если передали не правильные параметры</response>  
        /// 
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
