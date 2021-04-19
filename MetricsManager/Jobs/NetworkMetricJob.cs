using MetricsManager.Client;
using MetricsManager.DAL;
using MetricsManager.DAL.Interfaces;
using MetricsManager.Requests;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetricsManager.Jobs
{
    [DisallowConcurrentExecution]
    public class NetworkMetricJob : IJob
    {
        private readonly IMetricsRepository<NetworkMetric> _repository;

        private readonly IAgentsRepository _agentsRepository;

        private readonly IMetricsAgentClient _metricsAgentClient;

        private readonly ILogger<NetworkMetricJob> _logger;

        public NetworkMetricJob(IMetricsRepository<NetworkMetric> repository, IAgentsRepository agentsRepository, IMetricsAgentClient metricsAgentClient,
            ILogger<NetworkMetricJob> logger)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
            _logger.LogInformation("Nlog встроен в NetworkMetricJob");

        }

        public Task Execute(IJobExecutionContext context)
        {
            List<AgentInfo> agents = _agentsRepository.GetAgents();
            foreach (var agent in agents)
            {
                DateTimeOffset timeOfLastRequest = _repository.GetLastMetricTime(agent.AgentId);

                var metricsResponse = _metricsAgentClient.GetAllNetworkMetrics(new GetAllNetworkMetricsApiRequest
                {
                    AgentAddress = agent.AgentAddress,
                    FromTime = timeOfLastRequest,
                    ToTime = DateTimeOffset.UtcNow,
                });
                if (metricsResponse != null)
                {
                    foreach (var metric in metricsResponse.Metrics)
                    {
                        _repository.Create(
                            new NetworkMetric
                            {
                                AgentId = agent.AgentId,
                                Time = metric.Time,
                                Value = metric.Value
                            });
                    }
                }
                else
                {
                    _logger.LogError($"Network metrics response from agent(id {agent.AgentId}) was null");
                }
            }
            return Task.CompletedTask;
        }
    }

}