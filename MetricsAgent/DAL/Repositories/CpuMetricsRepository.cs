using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using MetricsAgent.DAL.Handlers;
using MetricsAgent.SQLSettingsProvider;

namespace MetricsAgent.DAL.Repositories
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private readonly ISqlSettingsProvider _sqlSettingsProvider;


        public CpuMetricsRepository(ISqlSettingsProvider sqlSettingsProvider)
        {
            _sqlSettingsProvider = sqlSettingsProvider;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }


        public void Create(CpuMetric item)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                connection.Execute("INSERT INTO cpumetrics(value, time) VALUES(@value, @time)",
                    new
                    {
                        value = item.Value,
                        time = item.Time.ToUnixTimeSeconds()
                    });
            }
        }


        public List<CpuMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                return connection.Query<CpuMetric>("SELECT Id, Time, Value FROM cpumetrics WHERE Time>=@fromTime AND Time<=@toTime",
                    new { fromTime = fromTime.ToUnixTimeSeconds(), toTime = toTime.ToUnixTimeSeconds() }).ToList();
            }
        }
    }
}
