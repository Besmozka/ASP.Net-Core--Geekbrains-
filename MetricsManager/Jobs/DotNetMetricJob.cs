using MetricsManager.DAL;
using Quartz;
using System;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;

namespace MetricsManager.Jobs
{
    public class DotNetMetricJob : IJob
    {
        private IMetricsRepository<DotNetMetric> _repository;

        public DotNetMetricJob(IMetricsRepository<DotNetMetric> repository)
        {
            _repository = repository;
        }

        public Task Execute(IJobExecutionContext context)
        {

            return Task.CompletedTask;
        }
    }

}