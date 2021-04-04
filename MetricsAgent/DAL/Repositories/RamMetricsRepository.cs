using Dapper;
using MetricsAgent.DAL.Handlers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsAgent.DAL
{
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

        public RamMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }
        public void Create(RamMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO rammetrics(value, time) VALUES(@value, @time)",
                    new
                    {
                        value = item.Value,
                        time = item.Time.ToUnixTimeSeconds()
                    });
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM rammetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public void Update(RamMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE rammetrics SET value = @value, time = @time WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        time = item.Time.ToUnixTimeSeconds(),
                        id = item.Id
                    });
            }
        }

        public List<RamMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<RamMetric>("SELECT Id, Time, Value FROM rammetrics").ToList();
            }
        }

        public RamMetric GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<RamMetric>("SELECT Id, Time, Value FROM rammetrics WHERE id=@id",
                    new { id = id });
            }
        }
    }
}
