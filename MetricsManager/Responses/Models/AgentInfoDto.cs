using System;

namespace MetricsManager.Responses.Models
{
    public class AgentInfoDto
    {
        public int AgentId { get; set; }

        public Uri AgentAddress { get; set; }
    }
}
