using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private INetworkMetricsRepository _repository;

        // счетчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public NetworkMetricJob(INetworkMetricsRepository repository)
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