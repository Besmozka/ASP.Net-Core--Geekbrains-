using System;

namespace MetricsManager.DAL
{
    public class AgentInfo
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public Uri AgentAddress { get; set; }
    }
}
