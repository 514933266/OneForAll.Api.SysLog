using AutoMapper;
using SysLog.Domain.Models;
using SysLog.Application.Dtos;
using SysLog.Domain.AggregateRoots;


namespace SysLog.Host.Profiles
{
    public class SysExceptionLogProfile : Profile
    {
        public SysExceptionLogProfile()
        {
            CreateMap<SysExceptionLog, SysExceptionLogDto>();
            CreateMap<SysExceptionLogForm, SysExceptionLog>()
                .ForMember(t => t.SysTenantId, a => a.MapFrom(s => s.TenantId));
        }
    }
}
