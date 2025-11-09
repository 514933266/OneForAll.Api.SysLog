using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OneForAll.Core;
using OneForAll.Core.Extension;
using SysLog.Host.Models;
using SysLog.Application.Interfaces;
using SysLog.Domain.Models;

namespace SysLog.Host.Filters
{
    /// <summary>
    /// 全局异常过滤器。
    /// 实现 IAsyncExceptionFilter 接口，用于捕获控制器中未处理的异常，
    /// 统一返回友好的错误响应，并将异常信息记录到日志系统。
    /// </summary>
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        /// <summary>
        /// 认证配置，用于标识当前模块（如客户端名称、编码）。
        /// </summary>
        private readonly AuthConfig _authConfig;

        /// <summary>
        /// 异常日志服务，用于将异常信息持久化存储。
        /// </summary>
        private readonly ISysExceptionLogService _logService;

        /// <summary>
        /// 构造函数，通过依赖注入获取所需服务。
        /// </summary>
        /// <param name="authConfig">认证配置</param>
        /// <param name="logService">异常日志服务</param>
        public ExceptionFilter(AuthConfig authConfig, ISysExceptionLogService logService)
        {
            _authConfig = authConfig;
            _logService = logService;
        }

        /// <summary>
        /// 异步处理异常的主方法。
        /// 当控制器或 Action 抛出未处理异常时被调用。
        /// </summary>
        /// <param name="context">异常上下文，包含异常对象、路由信息、HttpContext 等</param>
        /// <returns>异步任务</returns>
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            // 如果异常已被其他过滤器处理，则不再重复处理
            if (context.ExceptionHandled)
                return;

            // 构造统一的错误响应体
            var result = new BaseMessage
            {
                Status = false,
                ErrType = BaseErrType.ServerError,
                Message = context.Exception.Message
            };

            // 设置 HTTP 响应：状态码 200 + JSON 内容（注意：此处使用 200 可能不符合 REST 规范）
            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json;charset=utf-8",
                Content = result.ToJson()
            };

            // 获取发生异常的控制器和 Action 名称
            var controller = context.ActionDescriptor.RouteValues["controller"];
            var action = context.ActionDescriptor.RouteValues["action"];

            // 异步记录异常日志
            await _logService.AddAsync(new SysExceptionLogForm()
            {
                ModuleName = _authConfig.ClientName,
                ModuleCode = _authConfig.ClientCode,
                Controller = controller ?? "Unknown",
                Action = action ?? "Unknown",
                Name = context.Exception.Message ?? "无异常信息",
                Content = context.Exception.StackTrace ?? "无堆栈信息"
            });

            // 标记异常已处理，防止后续中间件再次处理
            context.ExceptionHandled = true;
        }
    }
}