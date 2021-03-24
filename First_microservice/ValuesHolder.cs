using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace First_microservice
{
    public class ValuesHolder
    {
        public List<WeatherForecast> Values;
        public ValuesHolder()
        {
            Values = new List<WeatherForecast>();
        }
    }
}
