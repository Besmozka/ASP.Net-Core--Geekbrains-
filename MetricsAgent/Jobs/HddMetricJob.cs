using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private IHddMetricsRepository _repository;

        // счетчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public HddMetricJob(IHddMetricsRepository repository)
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