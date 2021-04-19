using System;

namespace MetricsManager.Responses.Models
{
    public class HddMetricDto
    {
        public int Id { get; set; }

        public int AgentId { get; set; }

        public int Value { get; set; }

        public DateTimeOffset Time { get; set; }
    }
}
