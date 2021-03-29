using System;

namespace MetricsManager
{
    public class AgentInfo
    {
        public int AgentId { get; set; }

        public Uri AgentAddress { get; set; }

        public AgentInfo(int agentId, Uri agentAddress)
        {
            AgentId = agentId;
            AgentAddress = agentAddress;
        }
    }
}
