using Base.Host.Models;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using SysLog.Application.Interfaces;
using SysLog.Domain.AggregateRoots;
using SysLog.Domain.Models;
using SysLog.Public.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Host.Filters
{
    /// <summary>
    /// Api日志
    /// </summary>
    public class ApiLogMiddleware
    {
        private ISysApiLogService _service;

        private Stopwatch _stopWatch;
        private readonly AuthConfig _authConfig;
        private RequestDelegate _request;

        public ApiLogMiddleware(RequestDelegate request, AuthConfig authConfig, ISysApiLogService service)
        {
            _request = request;
            _authConfig = authConfig;
            _service = service;
            _stopWatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _stopWatch.Restart();

            var request = context.Request;
            var loginUser = GetLoginUser(context);
            var descriptor = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();

            var data = new SysApiLogForm()
            {
                MoudleName = _authConfig.ClientName,
                MoudleCode = _authConfig.ClientCode,
                CreatorId = loginUser.Id,
                CreatorName = loginUser.Name,
                TenantId = loginUser.SysTenantId,
                Host = request.Host.ToString(),
                Url = request.Path.ToString(),
                Method = request.Method.ToUpper(),
                ContentType = request.ContentType,
                UserAgent = request.Headers["User-Agent"],
                IPAddress = request.HttpContext.Connection.RemoteIpAddress.ToString(),
                Action = descriptor == null ? "" : descriptor.ActionName,
                Controller = descriptor == null ? "" : descriptor.ControllerName
            };

            data.RequestBody = await GetRequestBody(context.Request);
            data.ReponseBody = await GetResponseBody(context);

            // 响应完成记录时间和存入日志
            context.Response.OnCompleted(() =>
            {
                _stopWatch.Stop();

                data.TimeSpan = _stopWatch.Elapsed.ToString();
                data.StatusCode = context.Response.StatusCode.ToString();

                if (data.CreatorId != Guid.Empty)
                    _service.AddAsync(data);

                return Task.CompletedTask;
            }); ;
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<string> GetRequestBody(HttpRequest request)
        {
            return await Task.Run(() =>
            {
                var body = string.Empty;
                var method = request.Method.ToUpper();
                if (method.Equals("POST") || method.Equals("PATCH") || method.Equals("DELETED"))
                {
                    if (!request.HasFormContentType)
                    {
                        foreach (var item in request.Form)
                        {
                            body += $"{item.Key}:{item.Value},";
                        }
                        body = "{" + body.TrimEnd(',') + "}";
                    }
                    else if (request.HasJsonContentType())
                    {
                        var stream = request.Body;
                        var buffer = new byte[request.ContentLength.Value];
                        stream.Read(buffer, 0, buffer.Length);
                        body = Encoding.UTF8.GetString(buffer);
                    }
                }
                else if (method.Equals("GET"))
                {
                    body = request.QueryString.Value;
                }
                return body;
            });
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<string> GetResponseBody(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _request(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await context.Response.Body.CopyToAsync(originalBodyStream);
                return body;
            }
        }

        private LoginUser GetLoginUser(HttpContext context)
        {
            var role = context.User.Claims.FirstOrDefault(e => e.Type == UserClaimType.ROLE);
            var userId = context.User.Claims.FirstOrDefault(e => e.Type == UserClaimType.USER_ID);
            var name = context.User.Claims.FirstOrDefault(e => e.Type == UserClaimType.USER_NICKNAME);
            var tenantId = context.User.Claims.FirstOrDefault(e => e.Type == UserClaimType.TENANT_ID);

            return new LoginUser()
            {
                Id = userId == null ? Guid.Empty : new Guid(userId.Value),
                Name = name == null ? "" : name?.Value,
                SysTenantId = tenantId == null ? Guid.Empty : new Guid(tenantId?.Value),
                IsDefault = role == null ? false : role.Value.Equals(UserRoleType.RULER)
            };
        }
    }
}
