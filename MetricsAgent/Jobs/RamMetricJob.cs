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
            // получаем значение занятости CPU
            var ramCounter = Convert.ToInt32(_cpuCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow;

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new RamMetric { Time = time, Value = ramCounter });

            return Task.CompletedTask;
        }
    }

}