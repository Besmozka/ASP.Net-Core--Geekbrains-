using System.Collections.Generic;

namespace MetricsManager.DAL.Interfaces
{
    public interface IAgentsRepository
    {
        void Create(AgentInfo item);

        List<AgentInfo> GetAgents();
    }

}
