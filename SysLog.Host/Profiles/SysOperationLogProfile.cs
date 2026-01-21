using AutoMapper;
using SysLog.Application.Dtos;
using SysLog.Domain.Entities;
using SysLog.Domain.Models;

namespace SysLog.Host.Profiles
{
    public class SysOperationLogProfile : Profile
    {
        public SysOperationLogProfile()
        {
            CreateMap<SysOperationLog, SysOperationLogDto>();
            CreateMap<SysOperationLogForm, SysOperationLog>()
                .ForMember(t => t.SysTenantId, a => a.MapFrom(s => s.TenantId));
        }
    }
}
