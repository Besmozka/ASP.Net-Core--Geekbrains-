using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class DotNetMetricJob : IJob
    {
        private IDotNetMetricsRepository _repository;

        // счетчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public DotNetMetricJob(IDotNetMetricsRepository repository)
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