using MetricsManager.DAL;
using Quartz;
using System;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;

namespace MetricsManager.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private IMetricsRepository<NetworkMetric> _repository;

        public NetworkMetricJob(IMetricsRepository<NetworkMetric> repository)
        {
            _repository = repository;
        }

        public Task Execute(IJobExecutionContext context)
        {

            return Task.CompletedTask;
        }
    }

}