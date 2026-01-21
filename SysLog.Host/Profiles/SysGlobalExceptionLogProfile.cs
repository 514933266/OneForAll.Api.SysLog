using AutoMapper;
using SysLog.Domain.Models;
using SysLog.Application.Dtos;
using SysLog.Domain.Entities;


namespace SysLog.Host.Profiles
{
    public class SysGlobalExceptionLogProfile : Profile
    {
        public SysGlobalExceptionLogProfile()
        {
            CreateMap<SysGlobalExceptionLog, SysGlobalExceptionLogDto>();
            CreateMap<SysGlobalExceptionLogForm, SysGlobalExceptionLog>();
        }
    }
}
