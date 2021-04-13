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
    [DisallowConcurrentExecution] //аттрибут указывающий на то, что пока экземпляр этой Job'ы не отработает, следующий не запустится
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
                if (metricsResponse != null)
                {
                    foreach (var metric in metricsResponse.Metrics)
                    {
                        _repository.Create(
                            new CpuMetric
                            {
                                AgentId = agent.AgentId,
                                Time = metric.Time,
                                Value = metric.Value
                            });
                    }
                }
                else
                {
                    _logger.LogError($"CPU metrics response from agent(id {agent.AgentId}) was null");
                }
            }
            return Task.CompletedTask;
        }
    }

}