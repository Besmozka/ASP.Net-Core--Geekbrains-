﻿using AutoMapper;
using EnumsLibrary;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetricsAgent.Controllers
{

    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private ICpuMetricsRepository _repository;

        private readonly ILogger<CpuMetricsController> _logger;

        private readonly IMapper _mapper;
        public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug("Nlog встроен в CpuMetricsController");
            _mapper = mapper;
        }

        [HttpGet("from/{fromTime}/to/{toTime}/")]
        public IActionResult GetCpuMetricsTimeInterval([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetCpuMetricsTimeInterval - From time: {fromTime}; To time: {toTime}");

            List<CpuMetric> metrics = _repository.GetByTimePeriod(fromTime, toTime);

            var response = new AllMetricsResponse<CpuMetricDto>()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetCpuMetricsByPercentileTimeInterval([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            _logger.LogInformation($"GetCpuMetricsByPercentileTimeInterval - From time: {fromTime}; To time: {toTime}; Percentile: {percentile}");

            List<CpuMetric> metrics = _repository.GetByTimePeriod(fromTime, toTime).OrderBy(x => x.Value).ToList();

            var response = new AllMetricsResponse<CpuMetricDto>()
            {
                Metrics = new List<CpuMetricDto>()
            };

            int listPercentileIndex;

            switch (percentile)
            {
                case Percentile.Median:
                    listPercentileIndex = metrics.Count * 50 / 100;
                    break;
                case Percentile.P75:
                    listPercentileIndex = metrics.Count * 75 / 100;
                    break;
                case Percentile.P90:
                    listPercentileIndex = metrics.Count * 90 / 100;
                    break;
                case Percentile.P95:
                    listPercentileIndex = metrics.Count * 95 / 100;
                    break;
                case Percentile.P99:
                    listPercentileIndex = metrics.Count * 99 / 100;
                    break;
                default:
                    return BadRequest($"{percentile} не существует");                   
            }

            response.Metrics.Add(_mapper.Map<CpuMetricDto>(metrics[listPercentileIndex]));

            return Ok(response);
        }
    }
}
