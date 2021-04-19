using System.Collections.Generic;

namespace MetricsManager.DAL.Interfaces
{
    public interface IAgentsRepository
    {
        void Create(AgentInfo item);

        void Delete(string agentAddress);

        List<AgentInfo> GetAgents();
    }

}
