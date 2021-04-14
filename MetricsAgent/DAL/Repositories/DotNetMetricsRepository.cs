using Dapper;
using MetricsAgent.DAL.Handlers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using MetricsAgent.SQLSettingsProvider;

namespace MetricsAgent.DAL
{
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private readonly ISqlSettingsProvider _sqlSettingsProvider;


        public DotNetMetricsRepository(ISqlSettingsProvider sqlSettingsProvider)
        {
            _sqlSettingsProvider = sqlSettingsProvider;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }


        public void Create(DotNetMetric item)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                connection.Execute("INSERT INTO dotnetmetrics(value, time) VALUES(@value, @time)",
                    new
                    {
                        value = item.Value,
                        time = item.Time.ToUnixTimeSeconds()
                    });
            }
        }


        public List<DotNetMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                return connection.Query<DotNetMetric>("SELECT * FROM dotnetmetrics WHERE Time>=@fromTime AND Time<=@toTime",
                    new { fromTime = fromTime.ToUnixTimeSeconds(), toTime = toTime.ToUnixTimeSeconds() }).ToList();
            }
        }
    }
}
