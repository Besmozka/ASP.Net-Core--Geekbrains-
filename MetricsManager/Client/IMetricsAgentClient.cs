using MetricsManager.DAL;
using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client
{
    public interface IMetricsAgentClient
    {
        AllMetricsResponse<RamMetric> GetAllRamMetrics(GetAllRamMetricsApiRequest request);

        AllMetricsResponse<HddMetric> GetAllHddMetrics(GetAllHddMetricsApiRequest request);

        AllMetricsResponse<DotNetMetric> GetAllDotNetMetrics(GetAllDotNetMetricsApiRequest request);

        AllMetricsResponse<CpuMetric> GetAllCpuMetrics(GetAllCpuMetricsApiRequest request);

        AllMetricsResponse<NetworkMetric> GetAllNetworkMetrics(GetAllNetworkMetricsApiRequest request);
    }
}
