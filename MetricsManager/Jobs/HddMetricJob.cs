using MetricsManager.DAL;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repositories;

namespace MetricsManager.Jobs
{
    public class HddMetricJob : IJob
    {
        private IMetricsRepository<HddMetric> _repository;

        // счетчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public HddMetricJob(IMetricsRepository<HddMetric> repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Логический диск", "Свободно мегабайт", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var HddLeftSize = Convert.ToInt32(_cpuCounter.NextValue());

            var time = DateTimeOffset.UtcNow;

            _repository.Create(new HddMetric { Time = time, Value = HddLeftSize });

            return Task.CompletedTask;
        }
    }

}