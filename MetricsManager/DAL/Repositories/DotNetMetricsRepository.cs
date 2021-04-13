using Dapper;
using MetricsManager.DAL.Handlers;
using MetricsManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsManager.DAL.Repositories
{
    public class DotNetMetricsRepository : IMetricsRepository<DotNetMetric>
    {
        private const string ConnectionString = @"Data Source=ManagerMetrics.db";

        public DotNetMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void Create(DotNetMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO dotnetmetrics(agentid, value, time) VALUES(@agentid, @value, @time)",
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
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var metrics = connection.Query<DotNetMetric>("SELECT * FROM dotnetmetrics").ToList();
                if (metrics.Count != 0)
                {
                    return metrics.Max(item => item.Time);
                }
                return DateTimeOffset.MinValue;
            }
        }

        public List<DotNetMetric> GetAgentMetricByTimePeriod(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<DotNetMetric>("SELECT Id, AgentId, Time, Value FROM dotnetmetrics WHERE AgentId==@agentId AND Time>=@fromTime AND Time<=@toTime",
                    new { agentId = agentId, fromTime = fromTime.ToUnixTimeSeconds(), toTime = toTime.ToUnixTimeSeconds() }).ToList();
            }
        }

        public List<DotNetMetric> GetClusterMetricsByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<DotNetMetric>("SELECT Id, AgentId, Time, Value FROM dotnetmetrics WHERE Time>=@fromTime AND Time<=@toTime",
                    new { fromTime = fromTime.ToUnixTimeSeconds(), toTime = toTime.ToUnixTimeSeconds() }).ToList();
            }
        }
    }
}
