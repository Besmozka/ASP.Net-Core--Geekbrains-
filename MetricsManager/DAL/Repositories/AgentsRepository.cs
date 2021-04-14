using Dapper;
using MetricsManager.DAL.Handlers;
using MetricsManager.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using MetricsManager.SQLSettingsProvider;

namespace MetricsManager.DAL.Repositories
{
    public class AgentsRepository : IAgentsRepository
    {
        private readonly ISqlSettingsProvider _sqlSettingsProvider;


        public AgentsRepository(ISqlSettingsProvider sqlSettingsProvider)
        {
            _sqlSettingsProvider = sqlSettingsProvider;
            SqlMapper.AddTypeHandler(new UriHandler());
        }


        public void Create(AgentInfo item)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                connection.Execute("INSERT INTO agentslist(AgentAddress) VALUES(@address)",
                                        new { address = item.AgentAddress.ToString() });
            }
        }


        public void Delete(string agentAddress)
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                connection.Execute("DELETE FROM agentslist WHERE AgentAddress = @agentAddress",
                    new { agentAddress = agentAddress });
            }
        }


        public List<AgentInfo> GetAgents()
        {
            using (var connection = new SQLiteConnection(_sqlSettingsProvider.GetConnectionString()))
            {
                return connection.Query<AgentInfo>("SELECT * FROM agentslist").ToList();
            }
        }
    }
}
