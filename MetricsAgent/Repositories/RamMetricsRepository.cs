using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsAgent.DAL
{
    public class RamMetricsRepository : IRepository<RamMetric>
    {
        private SQLiteConnection connection;

        public RamMetricsRepository(SQLiteConnection connection)
        {
            this.connection = connection;
        }

        public void Create(RamMetric item)
        {
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(@value, @time)";

            cmd.Parameters.AddWithValue("@value", item.Value);

            cmd.Parameters.AddWithValue("@time", item.Time.ToUnixTimeSeconds());
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "DELETE FROM cpumetrics WHERE id=@id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(RamMetric item)
        {
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "UPDATE cpumetrics SET value = @value, time = @time WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.ToUnixTimeSeconds());
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public IList<RamMetric> GetAll()
        {
            using var cmd = new SQLiteCommand(connection);

            cmd.CommandText = "SELECT * FROM cpumetrics";

            var returnList = new List<RamMetric>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new RamMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(2))
                    });
                }
            }

            return returnList;
        }

        public RamMetric GetById(int id)
        {
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM cpumetrics WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new RamMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(2))
                    };
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
