using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.Requests;
using MetricsManager.Responses;
using MetricsManager.Responses.Models;

namespace MetricsManager.Client
{
    interface IMetricsAgentClient
    {
        AllMetricsResponse<RamMetricDto> GetAllRamMetrics(GetAllRamMetricsApiRequest request);

        AllMetricsResponse<HddMetricDto> GetAllHddMetrics(GetAllHddMetricsApiRequest request);

        AllMetricsResponse<DotNetMetricDto> GetDonNetMetrics(DonNetHeapMetrisApiRequest request);

        AllMetricsResponse<CpuMetricDto> GetCpuMetrics(GetAllCpuMetricsApiRequest request);
    }
}
