using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using MetricsManager.DAL.Handlers;
using MetricsManager.DAL.Interfaces;

namespace MetricsManager.DAL.Repositories
{
    public class AgentsRepository : IAgentsRepository
    {
        private const string ConnectionString = @"Data Source=ManagerMetrics.db";

        public AgentsRepository()
        {
            SqlMapper.AddTypeHandler(new UriHandler());
        }
        public void Create(AgentInfo item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO agentslist(AgentId, AgentAddress) VALUES(@agentId, @address)",
                                        new
                                        {
                                            agentId = item.AgentId,
                                            address = item.AgentAddress.ToString()
                                        });
            }
        }
        public List<AgentInfo> GetAgents()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<AgentInfo>("SELECT * FROM agentslist").ToList();
            }
        }
    }
}
