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
        private readonly ValuesHolder holder;

        public ValuesController(ValuesHolder holder)
        {
            this.holder = holder;
        }

        [HttpPost("create")]
        public IActionResult Create([FromQuery] string date, [FromQuery] int temperatureC)
        {
            var weatherForecast = new WeatherForecast();
            weatherForecast.Date = Convert.ToDateTime(date);
            weatherForecast.TemperatureC = temperatureC;
            holder.Values.Add(weatherForecast);
            return Ok();
        }

        [HttpGet("read")]
        public IActionResult Read([FromQuery] string dateFrom, [FromQuery] string dateTo)
        {
            var deltaDateValues = new List<WeatherForecast>();
            foreach (var value in holder.Values)
            {
                if (value.Date >= Convert.ToDateTime(dateFrom) && value.Date <= Convert.ToDateTime(dateTo))
                {
                    deltaDateValues.Add(value);
                }
            }
            return Ok(deltaDateValues);
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] string date, [FromQuery] int temperatureC)
        {
            foreach (var value in holder.Values)
            {
                if (value.Date == Convert.ToDateTime(date))
                {
                    value.TemperatureC = temperatureC;
                }
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] string date)
        {
            foreach (var value in holder.Values)
            {
                if (value.Date == Convert.ToDateTime(date))
                {
                    holder.Values.Remove(value);
                    break;
                }
            }
            return Ok();
        }
    }
}



