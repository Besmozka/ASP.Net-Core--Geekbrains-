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
    public class DotNetMetricJob : IJob
    {
        private readonly IMetricsRepository<DotNetMetric> _repository;

        private readonly IAgentsRepository _agentsRepository;

        private readonly IMetricsAgentClient _metricsAgentClient;

        private readonly ILogger<DotNetMetricJob> _logger;

        public DotNetMetricJob(IMetricsRepository<DotNetMetric> repository, IAgentsRepository agentsRepository, IMetricsAgentClient metricsAgentClient,
            ILogger<DotNetMetricJob> logger)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
            _logger.LogInformation("Nlog встроен в DotNetMetricJob");

        }

        public Task Execute(IJobExecutionContext context)
        {
            List<AgentInfo> agents = _agentsRepository.GetAgents();
            foreach (var agent in agents)
            {
                DateTimeOffset timeOfLastRequest = _repository.GetLastMetricTime(agent.AgentId);

                var metricsResponse = _metricsAgentClient.GetAllDotNetMetrics(new GetAllDotNetMetricsApiRequest
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
                            new DotNetMetric
                            {
                                AgentId = agent.AgentId,
                                Time = metric.Time,
                                Value = metric.Value
                            });
                    }
                }
                else
                {
                    _logger.LogError($"DotNet metrics response from agent(id {agent.AgentId}) was null");
                }
            }
            return Task.CompletedTask;
        }
    }

}