using Dapper;
using MetricsManager.DAL.Handlers;
using MetricsManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using MetricsManager.SQLSettingsProvider;

namespace MetricsManager.DAL.Repositories
{
    public class NetworkMetricsRepository : IMetricsRepository<NetworkMetric>
    {
        private readonly ISqlSettingsProvider _sqlSettingsProvider;


        public NetworkMetricsRepository(ISqlSettingsProvider sqlSettingsProvider)
        {
            _sqlSettingsProvider = sqlSettingsProvider;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }


        public void Create(NetworkMetric item)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                connection.Execute("INSERT INTO networkmetrics(agentid, value, time) VALUES(@agentid, @value, @time)",
                    new
                    {
                        agentid = item.AgentId,
                        value = item.Value,
                        time = item.Time.ToUnixTimeSeconds()
                    });
            }
        }


        public DateTimeOffset GetLastMetricTime(int agentId)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                var metrics = connection.Query<NetworkMetric>("SELECT * FROM networkmetrics").ToList();
                if (metrics.Count != 0)
                {
                    return metrics.Max(item => item.Time);
                }
                return DateTimeOffset.MinValue;
            }
        }


        public List<NetworkMetric> GetAgentMetricByTimePeriod(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                return connection.Query<NetworkMetric>("SELECT Id, AgentId, Time, Value FROM networkmetrics WHERE AgentId==@agentId AND Time>=@fromTime AND Time<=@toTime",
                    new { agentId = agentId, fromTime = fromTime.ToUnixTimeSeconds(), toTime = toTime.ToUnixTimeSeconds() }).ToList();
            }
        }

        public List<NetworkMetric> GetClusterMetricsByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                return connection.Query<NetworkMetric>("SELECT Id, AgentId, Time, Value FROM networkmetrics WHERE Time>=@fromTime AND Time<=@toTime",
                    new { fromTime = fromTime.ToUnixTimeSeconds(), toTime = toTime.ToUnixTimeSeconds() }).ToList();
            }
        }
    }
}
