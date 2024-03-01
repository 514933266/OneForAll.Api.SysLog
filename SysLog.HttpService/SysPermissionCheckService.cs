using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using SysLog.HttpService.Models;
using SysLog.HttpService.Interfaces;
using SysLog.Public.Models;
using OneForAll.Core.OAuth;

namespace SysLog.HttpService
{
    /// <summary>
    /// 功能权限校验服务
    /// </summary>
    public class SysPermissionCheckHttpService : ISysPermissionCheckHttpService
    {
        private readonly string AUTH_KEY = "Authorization";
        private readonly HttpServiceConfig _config;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public SysPermissionCheckHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpContext = httpContext;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 登录token
        /// </summary>
        private string Token
        {
            get
            {
                var context = _httpContext.HttpContext;
                if (context != null)
                {
                    return context.Request.Headers
                      .FirstOrDefault(w => w.Key.Equals(AUTH_KEY))
                      .Value.TryString();
                }
                return "";
            }
        }

        /// <summary>
        /// 验证功能权限
        /// </summary>
        /// <returns>返回消息</returns>
        public async Task<BaseMessage> ValidateAuthorization(string controller, string action)
        {
            if (!Token.IsNullOrEmpty())
            {
                var claims = _httpContext.HttpContext.User.Claims;
                var uid = claims.FirstOrDefault(e => e.Type == UserClaimType.USER_ID).Value;

                var client = _httpClientFactory.CreateClient(_config.SysPermissionCheck);
                client.DefaultRequestHeaders.Add(AUTH_KEY, Token);
                var postData = new PermissionCheck()
                {
                    SysUserId = new Guid(uid),
                    Controller = controller,
                    Action = action
                };
                var result = await client.PostAsync(client.BaseAddress, postData, new JsonMediaTypeFormatter());
                return await result.Content.ReadAsAsync<BaseMessage>();
            }
            return new BaseMessage()
            {
                Status = false,
                ErrType = BaseErrType.TokenInvalid,
                Message = "登录已失效，权限验证失败"
            };
        }
    }
}
