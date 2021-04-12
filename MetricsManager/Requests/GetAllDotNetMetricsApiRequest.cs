using System;

namespace MetricsManager.Requests
{
    public class GetAllDotNetMetricsApiRequest
    {
        public Uri AgentAddress { get; set; }
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}
