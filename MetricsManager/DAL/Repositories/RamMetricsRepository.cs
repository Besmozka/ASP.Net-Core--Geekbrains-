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
    public class RamMetricsRepository : IMetricsRepository<RamMetric>
    {
        private readonly ISqlSettingsProvider _sqlSettingsProvider;

        public RamMetricsRepository(ISqlSettingsProvider sqlSettingsProvider)
        {
            _sqlSettingsProvider = sqlSettingsProvider;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void Create(RamMetric item)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                connection.Execute("INSERT INTO rammetrics(agentid, value, time) VALUES(@agentid, @value, @time)",
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
                var metrics = connection.QuerySingle<CpuMetric>("SELECT MAX(Time) FROM rammetrics WHERE AgentId = @agentId",
                    new { agentId = agentId });
                if (metrics != null)
                {
                    return metrics.Time;
                }
                return DateTimeOffset.MinValue;
            }
        }

        public List<RamMetric> GetAgentMetricByTimePeriod(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                var h = connection.Query<CpuMetric>("SELECT * FROM rammetrics");
                return connection.Query<RamMetric>("SELECT Id, AgentId, Time, Value FROM rammetrics WHERE AgentId==@agentId AND Time>=@fromTime AND Time<=@toTime",
                    new { agentId = agentId, fromTime = fromTime.ToUnixTimeSeconds(), toTime = toTime.ToUnixTimeSeconds() }).ToList();
            }
        }

        public List<RamMetric> GetClusterMetricsByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                return connection.Query<RamMetric>("SELECT Id, AgentId, Time, Value FROM rammetrics WHERE Time>=@fromTime AND Time<=@toTime",
                    new { fromTime = fromTime.ToUnixTimeSeconds(), toTime = toTime.ToUnixTimeSeconds() }).ToList();
            }
        }
    }
}
