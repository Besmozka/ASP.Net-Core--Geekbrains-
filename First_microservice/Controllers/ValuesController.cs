using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace First_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ValuesHolder _holder;

        public ValuesController(ValuesHolder holder)
        {
            this._holder = holder;
        }

        [HttpPost("create")]
        public IActionResult Create([FromQuery] DateTime date, [FromQuery] int temperatureC)
        {
            var weatherForecast = new WeatherForecast();
            weatherForecast.Date = date;
            weatherForecast.TemperatureC = temperatureC;
            _holder.Values.Add(weatherForecast);
            return Ok();
        }

        [HttpGet("read")]
        public IActionResult Read([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            var deltaDateValues = new List<WeatherForecast>();
            foreach (var value in _holder.Values)
            {
                if (value.Date >= dateFrom && value.Date <= dateTo)
                {
                    deltaDateValues.Add(value);
                }
            }
            return Ok(deltaDateValues);
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] DateTime date, [FromQuery] int temperatureC)
        {
            foreach (var value in _holder.Values)
            {
                if (value.Date == date)
                {
                    value.TemperatureC = temperatureC;
                }
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            for (int i = 0; i < _holder.Values.Count; i++)
            {
                if (_holder.Values[i].Date >= dateFrom && _holder.Values[i].Date <= dateTo)
                {
                    _holder.Values.Remove(_holder.Values[i]);
                    i--;
                }
            }
            return Ok();
        }
    }
}



