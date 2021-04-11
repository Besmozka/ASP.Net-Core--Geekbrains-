using MetricsManager.DAL;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repositories;

namespace MetricsManager.Jobs
{
    public class RamMetricJob : IJob
    {
        private IMetricsRepository<RamMetric> _repository;

        // счетчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public RamMetricJob(IMetricsRepository<RamMetric> repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Память", "Доступно МБ");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var ramAvialable = Convert.ToInt32(_cpuCounter.NextValue());

            var time = DateTimeOffset.UtcNow;

            _repository.Create(new RamMetric { Time = time, Value = ramAvialable });

            return Task.CompletedTask;
        }
    }

}