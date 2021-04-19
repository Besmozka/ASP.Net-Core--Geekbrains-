namespace MetricsAgent.SQLSettingsProvider
{
    public class SqlSettingsProvider: ISqlSettingsProvider
    {
        private string _connectionString = @"Data Source=AgentMetrics.db";
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
