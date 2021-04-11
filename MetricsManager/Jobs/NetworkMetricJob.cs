using MetricsManager.DAL;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repositories;

namespace MetricsManager.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private IMetricsRepository<NetworkMetric> _repository;

        // счетчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public NetworkMetricJob(IMetricsRepository<NetworkMetric> repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Сетевой интерфейс", "Текущая пропускная способность",
                "Realtek PCIe GbE Family Controller");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var NetworkDataTransferRate = Convert.ToInt32(_cpuCounter.NextValue());

            var time = DateTimeOffset.UtcNow;

            _repository.Create(new NetworkMetric { Time = time, Value = NetworkDataTransferRate });

            return Task.CompletedTask;
        }
    }

}