using MetricsManager.DAL;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MetricsAgentClient> _logger;

        public MetricsAgentClient(HttpClient httpClient, ILogger<MetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public AllMetricsResponse<HddMetric> GetAllHddMetrics(GetAllHddMetricsApiRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.AgentAddress}api/metrics/hdd/left");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllMetricsResponse<HddMetric>>(responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public AllMetricsResponse<RamMetric> GetAllRamMetrics(GetAllRamMetricsApiRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.AgentAddress}api/metrics/ram/available");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllMetricsResponse<RamMetric>>(responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public AllMetricsResponse<CpuMetric> GetAllCpuMetrics(GetAllCpuMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var toParameter = request.ToTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.AgentAddress}api/metrics/cpu/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllMetricsResponse<CpuMetric>>(responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public AllMetricsResponse<NetworkMetric> GetAllNetworkMetrics(GetAllNetworkMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var toParameter = request.ToTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.AgentAddress}api/metrics/network/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllMetricsResponse<NetworkMetric>>(responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public AllMetricsResponse<DotNetMetric> GetAllDotNetMetrics(GetAllDotNetMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var toParameter = request.ToTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.AgentAddress}api/metrics/dotnet/errors-count/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllMetricsResponse<DotNetMetric>>(responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
