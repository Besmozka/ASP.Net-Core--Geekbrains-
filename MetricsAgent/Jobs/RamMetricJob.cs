using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository _repository;

        // счетчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public RamMetricJob(IRamMetricsRepository repository)
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