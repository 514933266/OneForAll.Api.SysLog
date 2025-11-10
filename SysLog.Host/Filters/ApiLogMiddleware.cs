using OneForAll.Core.Extension;
using SysLog.Application.Interfaces;
using SysLog.Domain.Models;
using SysLog.Host.Models;
using SysLog.Public.Models;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Host.Filters
{
    /// <summary>
    /// API 日志中间件。
    /// 用于在请求进入和响应返回过程中自动记录 API 调用日志，包括请求参数、响应内容、执行耗时、用户信息等。
    /// 通过注入日志服务将数据异步持久化。
    /// </summary>
    public class ApiLogMiddleware
    {
        private readonly Stopwatch _stopWatch;
        private readonly RequestDelegate _next;
        private readonly AuthConfig _authConfig;

        private readonly ISysApiLogService _service;

        /// <summary>
        /// 构造函数，通过依赖注入初始化中间件所需的服务和配置。
        /// </summary>
        /// <param name="next">下一个中间件</param>
        /// <param name="authConfig">认证配置</param>
        /// <param name="service">API 日志服务</param>
        public ApiLogMiddleware(RequestDelegate next, AuthConfig authConfig, ISysApiLogService service)
        {
            _next = next;
            _authConfig = authConfig;
            _service = service;
            _stopWatch = new Stopwatch();
        }

        /// <summary>
        /// 中间件主入口方法，在每次 HTTP 请求时被调用。
        /// 负责收集请求上下文、用户信息、请求/响应体，并在响应完成后记录完整日志。
        /// </summary>
        /// <param name="context">当前 HTTP 上下文</param>
        /// <returns>异步任务</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // 重置并启动计时器，用于记录 API 执行时间
            _stopWatch.Restart();

            // 尝试从当前终结点（Endpoint）中获取控制器和动作元数据
            var descriptor = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();

            // 获取当前登录用户信息（从 Claims 中解析）
            var loginUser = GetLoginUser(context);

            // 获取 User-Agent 请求头
            var userAgent = context.Request.Headers["User-Agent"];

            // 初始化日志实体
            var data = new SysApiLogForm()
            {
                ModuleName = _authConfig.ClientName,
                ModuleCode = _authConfig.ClientCode,
                CreatorId = loginUser.Id.ToString(),
                CreatorName = loginUser.Name,
                TenantId = loginUser.SysTenantId.ToString(),
                Host = context.Request.Host.ToString(),
                Url = context.Request.Path.ToString(),
                Method = context.Request.Method.ToUpper(),
                ContentType = context.Request.ContentType ?? "application/json",
                UserAgent = string.IsNullOrEmpty(userAgent) ? "无" : userAgent.ToString(),
                IPAddress = context.Connection.RemoteIpAddress?.ToString() ?? "未知",
                Action = descriptor?.ActionName ?? "无",
                Controller = descriptor?.ControllerName ?? "无"
            };

            // 异步读取请求体内容（如 POST/PUT 的 JSON 数据）
            data.RequestBody = await GetRequestBody(context);

            // 异步读取响应体内容（需替换 Response.Body 为内存流）
            data.ReponseBody = await GetResponseBody(context);

            // 注册响应完成回调：在响应发送完毕后记录耗时和状态码，并保存日志
            context.Response.OnCompleted(async () =>
            {
                _stopWatch.Stop();
                data.TimeSpan = _stopWatch.Elapsed.ToString(@"hh\:mm\:ss\.fff"); // 格式化为可读时间
                data.StatusCode = context.Response.StatusCode.ToString();

                // 异步保存日志（注意：OnCompleted 中应避免长时间阻塞）
                _service.AddAsync(data);
            });
        }

        /// <summary>
        /// 从 HttpContext 中提取请求体内容。
        /// 支持 GET（查询字符串）和常见写操作（POST/PUT/PATCH 等）。
        /// 对于 multipart/form-data 表单上传，暂不读取以避免性能问题。
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <returns>请求体字符串</returns>
        private async Task<string> GetRequestBody(HttpContext context)
        {
            var method = context.Request.Method.ToUpperInvariant();
            var writeMethods = new[] { "POST", "PUT", "PATCH", "DELETE" };

            if (writeMethods.Contains(method))
            {
                // 跳过多部分表单（如文件上传），避免 BodyReader 异常或性能损耗
                if (!(context.Request.ContentType?.StartsWith("multipart/form-data") == true &&
                      context.Request.HasFormContentType))
                {
                    // 启用请求体缓冲，以便多次读取
                    context.Request.EnableBuffering();

                    // 读取原始请求体
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
                    {
                        var body = await reader.ReadToEndAsync();
                        context.Request.Body.Seek(0, SeekOrigin.Begin); // 重置流位置，供后续中间件使用
                        return body;
                    }
                }
            }
            else if (method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                // GET 请求记录查询字符串
                return context.Request.QueryString.Value ?? string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// 拦截并读取响应体内容。
        /// 通过临时替换 Response.Body 为内存流，捕获控制器返回的内容。
        /// 注意：此操作会影响性能，且不适用于流式响应（如文件下载、SSE）。
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <returns>响应体字符串</returns>
        private async Task<string> GetResponseBody(HttpContext context)
        {
            // 保存原始响应流
            var originalBodyStream = context.Response.Body;

            // 使用内存流临时接收响应
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // 继续执行后续中间件（包括控制器逻辑）
                await _next(context);

                // 读取内存流中的响应内容
                responseBody.Seek(0, SeekOrigin.Begin);
                var body = await new StreamReader(responseBody).ReadToEndAsync();

                // 将内容写回原始响应流，确保客户端能收到正确响应
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);

                return body;
            }
        }

        /// <summary>
        /// 从当前用户的 Claims 中提取登录用户信息。
        /// 若某些 Claim 不存在，则使用默认值（如空 Guid、"无" 等）。
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <returns>封装后的用户信息对象</returns>
        private LoginUserClaim GetLoginUser(HttpContext context)
        {
            var claims = context.User.Claims;
            var userIdClaim = claims.FirstOrDefault(e => e.Type == UserClaimType.UserId);
            var nameClaim = claims.FirstOrDefault(e => e.Type == UserClaimType.UserNickName);
            var tenantIdClaim = claims.FirstOrDefault(e => e.Type == UserClaimType.TenantId);
            var isDefaultClaim = claims.FirstOrDefault(e => e.Type == UserClaimType.IsDefault);

            return new LoginUserClaim()
            {
                Id = userIdClaim?.Value == null ? Guid.Empty : Guid.Parse(userIdClaim.Value),
                Name = nameClaim?.Value ?? "无",
                SysTenantId = tenantIdClaim?.Value == null ? Guid.Empty : Guid.Parse(tenantIdClaim.Value),
                IsDefault = isDefaultClaim?.TryBoolean() ?? false // 假设 TryBoolean 是扩展方法
            };
        }
    }
}