using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class AllMetricsResponse<T>
    {
        public List<T> Metrics { get; set; }
    }
}
