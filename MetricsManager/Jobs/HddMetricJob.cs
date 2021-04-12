using MetricsManager.DAL;
using Quartz;
using System;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;

namespace MetricsManager.Jobs
{
    public class HddMetricJob : IJob
    {
        private IMetricsRepository<HddMetric> _repository;

        public HddMetricJob(IMetricsRepository<HddMetric> repository)
        {
            _repository = repository;
        }

        public Task Execute(IJobExecutionContext context)
        {

            return Task.CompletedTask;
        }
    }

}