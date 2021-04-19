using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.SQLSettingsProvider
{
    public class SqlSettingsProvider: ISqlSettingsProvider
    {
        private string _connectionString = @"Data Source=ManagerMetrics.db";
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
