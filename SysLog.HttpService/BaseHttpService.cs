using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OneForAll.Core.Extension;
using SysLog.Public.Models;

namespace SysLog.HttpService
{
    public class BaseHttpService
    {
        private readonly string AUTH_KEY = "Authorization";
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IHttpClientFactory _httpClientFactory;
        public BaseHttpService(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 登录token
        /// </summary>
        protected string Token
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                if (context != null)
                {
                    return context.Request.Headers
                      .FirstOrDefault(w => w.Key.Equals(AUTH_KEY))
                      .Value.TryString();
                }
                return "";
            }
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

        /// <summary>
        /// 获取HttpClient
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected HttpClient GetHttpClient(string name)
        {
            var client = _httpClientFactory.CreateClient(name);
            if (!Token.IsNullOrEmpty())
            {
                client.DefaultRequestHeaders.Add(AUTH_KEY, Token);
            }
            return client;
        }
    }
}
