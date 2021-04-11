using MetricsManager.DAL;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repositories;

namespace MetricsManager.Jobs
{
    public class DotNetMetricJob : IJob
    {
        private IMetricsRepository<DotNetMetric> _repository;

        // счетчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public DotNetMetricJob(IMetricsRepository<DotNetMetric> repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Приложения ASP.NET", "Общее число ошибок", "__Total__");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var DotNetErrorsCount = Convert.ToInt32(_cpuCounter.NextValue());

            var time = DateTimeOffset.UtcNow;

            _repository.Create(new DotNetMetric { Time = time, Value = DotNetErrorsCount });

            return Task.CompletedTask;
        }
    }

}