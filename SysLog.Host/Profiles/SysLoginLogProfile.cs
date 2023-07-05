using AutoMapper;
using SysLog.Domain.Models;
using SysLog.Application.Dtos;
using SysLog.Domain.AggregateRoots;


namespace SysLog.Host.Profiles
{
    public class SysLoginLogProfile : Profile
    {
        public SysLoginLogProfile()
        {
            CreateMap<SysLoginLog, SysLoginLogDto>();
            CreateMap<SysLoginLogForm, SysLoginLog>()
                .ForMember(t => t.SysTenantId, a => a.MapFrom(s => s.TenantId));
        }
    }
}
