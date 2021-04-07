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
            _cpuCounter = new PerformanceCounter("Приложения ASP.NET", "Общее количество ошибок", "_Total_");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var DotNetUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow;

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new DotNetMetric { Time = time, Value = DotNetUsageInPercents });

            return Task.CompletedTask;
        }
    }

}