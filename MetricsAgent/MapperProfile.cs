using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>();
            CreateMap<CpuMetricDto, CpuMetric>();

            CreateMap<DotNetMetric, DotNetMetricDto>();
            CreateMap<DotNetMetricDto, DotNetMetric>();

            CreateMap<HddMetric, HddMetricDto>();
            CreateMap<HddMetricDto, HddMetric>();

            CreateMap<NetworkMetric, NetworkMetricDto>();
            CreateMap<NetworkMetricDto, NetworkMetric>();

            CreateMap<RamMetric, RamMetricDto>();
            CreateMap<RamMetricDto, RamMetric>();
        }
    }
}
