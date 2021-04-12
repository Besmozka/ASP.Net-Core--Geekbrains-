using System;

namespace MetricsManager.Requests
{
    public class GetAllNetworkMetricsApiRequest
    {
        public Uri AgentAddress { get; set; }
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}
