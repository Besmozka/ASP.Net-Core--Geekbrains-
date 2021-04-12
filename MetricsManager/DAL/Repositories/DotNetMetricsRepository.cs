using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using MetricsManager.DAL.Handlers;
using MetricsManager.DAL.Interfaces;

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
                return DateTimeOffset.FromUnixTimeSeconds(connection
                    .Query<DotNetMetric>("SELECT * FROM dotnetmetrics")
                    .Max(item => Convert.ToInt64(item.Time)));
            }
        }

        public List<DotNetMetric> GetAgentMetricByTimePeriod(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<DotNetMetric>("SELECT Id, AgentId, Time, Value FROM dotnetmetrics WHERE Id==@id Time>=@fromTime AND Time<=@toTime",
                    new { id = agentId, fromTime = fromTime.ToUnixTimeSeconds(), toTime = toTime.ToUnixTimeSeconds() }).ToList();
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
