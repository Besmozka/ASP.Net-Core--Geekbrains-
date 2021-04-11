using System;
using System.Collections.Generic;

namespace MetricsManager.DAL.Interfaces
{
    public interface IMetricsRepository<T> where T : class
    {
        List<T> GetAgentMetricByTimePeriod(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime);

        List<T> GetClusterMetricsByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime);

        void Create(T item);
        
    }

}
