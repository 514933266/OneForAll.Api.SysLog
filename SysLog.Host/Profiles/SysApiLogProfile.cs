using AutoMapper;
using SysLog.Domain.Models;
using SysLog.Application.Dtos;
using SysLog.Domain.AggregateRoots;


namespace SysLog.Host.Profiles
{
    public class SysApiLogProfile : Profile
    {
        public SysApiLogProfile()
        {
            CreateMap<SysApiLog, SysApiLogDto>();
            CreateMap<SysApiLogForm, SysApiLog>()
                .ForMember(t => t.SysTenantId, a => a.MapFrom(s => s.TenantId));
        }
    }
}
