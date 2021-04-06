using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class CpuMetricJob : IJob
    {
        // Инжектируем DI провайдер
        private readonly IServiceProvider _provider;
        private ICpuMetricsRepository _repository;

        public CpuMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<ICpuMetricsRepository>();
        }

        public Task Execute(IJobExecutionContext context)
        {
            // теперь можно записать что-то при помощи репозитория

            return Task.CompletedTask;
        }
    }
}