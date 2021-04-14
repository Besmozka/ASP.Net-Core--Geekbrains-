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
    public class HddMetricsRepository : IMetricsRepository<HddMetric>
    {
        private readonly ISqlSettingsProvider _sqlSettingsProvider;


        public HddMetricsRepository(ISqlSettingsProvider sqlSettingsProvider)
        {
            _sqlSettingsProvider = sqlSettingsProvider;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }


        public void Create(HddMetric item)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                connection.Execute("INSERT INTO hddmetrics(agentid, value, time) VALUES(@agentid, @value, @time)",
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
                var metrics = connection.Query<HddMetric>("SELECT * FROM hddmetrics").ToList();
                if (metrics.Count != 0)
                {
                    return metrics.Max(item => item.Time);
                }
                return DateTimeOffset.MinValue;
            }
        }


        public List<HddMetric> GetAgentMetricByTimePeriod(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                return connection.Query<HddMetric>("SELECT Id, AgentId, Time, Value FROM hddmetrics WHERE AgentId==@agentId AND Time>=@fromTime AND Time<=@toTime",
                    new { agentId = agentId, fromTime = fromTime.ToUnixTimeSeconds(), toTime = toTime.ToUnixTimeSeconds() }).ToList();
            }
        }


        public List<HddMetric> GetClusterMetricsByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                return connection.Query<HddMetric>("SELECT Id, AgentId, Time, Value FROM hddmetrics WHERE Time>=@fromTime AND Time<=@toTime",
                    new { fromTime = fromTime.ToUnixTimeSeconds(), toTime = toTime.ToUnixTimeSeconds() }).ToList();
            }
        }
    }
}
