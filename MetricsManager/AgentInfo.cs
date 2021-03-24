using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
