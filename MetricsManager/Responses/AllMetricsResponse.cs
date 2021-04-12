using System.Collections.Generic;

namespace MetricsManager.Responses
{
    public class AllMetricsResponse<T>
    {
        public List<T> Metrics { get; set; }
    }
}
