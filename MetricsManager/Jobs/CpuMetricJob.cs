using MetricsManager.DAL;
using Quartz;
using System;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using MetricsManager.Requests;
using System.Collections.Generic;
using MetricsManager.Client;

namespace MetricsManager.Jobs
{
    public class CpuMetricJob : IJob
    {
        private readonly IMetricsRepository<CpuMetric> _repository;

        private readonly IAgentsRepository _agentsRepository;

        private readonly IMetricsAgentClient _metricsAgentClient;

        private readonly ILogger<CpuMetricJob> _logger;

        public CpuMetricJob(IMetricsRepository<CpuMetric> repository, IAgentsRepository agentsRepository, IMetricsAgentClient metricsAgentClient,
            ILogger<CpuMetricJob> logger)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
            _logger.LogInformation("Nlog встроен в CpuMetricJob");

        }

        public Task Execute(IJobExecutionContext context)
        {
            List<AgentInfo> agents = _agentsRepository.GetAgents();
            foreach (var agent in agents)
            {
                DateTimeOffset timeOfLastRequest = _repository.GetLastMetricTime(agent.AgentId);

                var metricsResponse = _metricsAgentClient.GetAllCpuMetrics(new GetAllCpuMetricsApiRequest
                {
                    AgentAddress = agent.AgentAddress,
                    FromTime = timeOfLastRequest,
                    ToTime = DateTimeOffset.UtcNow,
                });

                foreach (var metric in metricsResponse.Metrics)
                {
                    _repository.Create(metric);
                }
            }
            return Task.CompletedTask;
        }
    }

}