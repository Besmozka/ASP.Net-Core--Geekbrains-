using MetricsManager.DAL;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repositories;

namespace MetricsManager.Jobs
{
    public class CpuMetricJob : IJob
    {
        private IMetricsRepository<CpuMetric> _repository;

        // счетчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public CpuMetricJob(IMetricsRepository<CpuMetric> repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Процессор", "% загруженности процессора", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var cpuUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow;

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new CpuMetric { Time = time, Value = cpuUsageInPercents });

            return Task.CompletedTask;
        }
    }

}