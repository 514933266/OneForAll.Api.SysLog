using AutoMapper;
using Microsoft.AspNetCore.Http;
using SysLog.Public.Models;
using OneForAll.Core.DDD;
using System.Linq;
using OneForAll.Core.Extension;

namespace SysLog.Domain
{
    /// <summary>
    /// 基类
    /// </summary>
    public class SysBaseManager : BaseManager
    {
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public SysBaseManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        protected LoginUserClaim LoginUser
        {
            get
            {
                var claims = _httpContextAccessor.HttpContext?.User.Claims;
                if (claims.Any())
                {
                    return new LoginUserClaim()
                    {
                        Name = claims.FirstOrDefault(e => e.Type == UserClaimType.UserNickName)?.Value ?? "",
                        UserName = claims.FirstOrDefault(e => e.Type == UserClaimType.UserName)?.Value ?? "",
                        WxAppId = claims.FirstOrDefault(e => e.Type == UserClaimType.WxAppId)?.Value ?? "",
                        WxOpenId = claims.FirstOrDefault(e => e.Type == UserClaimType.WxOpenId)?.Value ?? "",
                        WxUnionId = claims.FirstOrDefault(e => e.Type == UserClaimType.WxUnionId)?.Value ?? "",
                        Id = claims.FirstOrDefault(e => e.Type == UserClaimType.UserId).Value.TryGuid(),
                        SysTenantId = claims.FirstOrDefault(e => e.Type == UserClaimType.TenantId).Value.TryGuid(),
                        IsDefault = claims.FirstOrDefault(e => e.Type == UserClaimType.IsDefault).Value.TryBoolean()
                    };
                }
                return new LoginUserClaim();
            }
        }
    }
}
