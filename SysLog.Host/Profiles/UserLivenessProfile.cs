using AutoMapper;
using SysLog.Application.Dtos;
using SysLog.Domain.Entities;
using SysLog.Domain.Aggregates;
using SysLog.Domain.Models;

namespace SysLog.Host.Profiles
{
    public class UserLivenessProfile : Profile
    {
        public UserLivenessProfile()
        {
            CreateMap<UserLivenessAggr, UserLivenessDto>();
        }
    }
}
