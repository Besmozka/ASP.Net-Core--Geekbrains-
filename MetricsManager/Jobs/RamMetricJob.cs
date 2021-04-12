using MetricsManager.DAL;
using Quartz;
using System;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;

namespace MetricsManager.Jobs
{
    public class RamMetricJob : IJob
    {
        private IMetricsRepository<RamMetric> _repository;

        public RamMetricJob(IMetricsRepository<RamMetric> repository)
        {
            _repository = repository;
        }

        public Task Execute(IJobExecutionContext context)
        {

            return Task.CompletedTask;
        }
    }

}