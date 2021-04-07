using AutoMapper;
using MetricsAgent;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private IHddMetricsRepository _repository;

        private readonly ILogger<HddMetricsController> _logger;

        private readonly IMapper _mapper;
        public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug("Nlog встроен в HddMetricsController");
            _mapper = mapper;
        }

        [HttpGet("hdd/left")]
        public IActionResult GetHddSizeLeft()
        {
            _logger.LogInformation($"GetHddSizeLeft - Left:");
            return Ok();
        }
    }
}
