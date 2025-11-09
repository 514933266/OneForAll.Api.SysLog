using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using SysLog.Host.Models;
using SysLog.HttpService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// 自定义授权过滤器，用于在执行控制器方法前进行权限校验。
/// 实现了 IAuthorizationFilter 接口，在 ASP.NET Core 管道中拦截请求并验证用户是否有权访问目标资源。
/// </summary>
public class AuthorizationFilter : IAuthorizationFilter
{

    private readonly AuthConfig _config;
    private readonly ISysPermissionCheckHttpService _httpPermService;

    /// <summary>
    /// 构造函数，通过依赖注入注入配置和服务。
    /// </summary>
    /// <param name="config">认证配置信息</param>
    /// <param name="httpPermService">权限校验 HTTP 服务</param>
    public AuthorizationFilter(AuthConfig config, ISysPermissionCheckHttpService httpPermService)
    {
        _config = config;
        _httpPermService = httpPermService;
    }

    /// <summary>
    /// 在授权阶段执行的主逻辑。
    /// 检查是否允许匿名访问、是否跳过权限检查（通过特殊 Header），以及是否需要进行权限验证。
    /// </summary>
    /// <param name="context">授权过滤器上下文，包含当前请求和路由等信息</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // 如果存在 [AllowAnonymous] 特性，或者当前不是控制器方法，则跳过权限检查
        if (context.Filters.Any(item => item is IAllowAnonymousFilter) ||
            !(context.ActionDescriptor is ControllerActionDescriptor))
        {
            return;
        }

        // 检查请求头中是否包含 "Unchecked" 字段，用于临时绕过权限校验（需签名验证）
        var unChecked = context.HttpContext.Request.Headers["Unchecked"];
        if (!unChecked.IsNull())
        {
            // 构造签名字符串：clientId + clientSecret + 当前时间（格式：yyyyMMddhhmm）
            var sign = "clientId={0}&clientSecret={1}&tt={2}"
                .Fmt(_config.ClientId, _config.ClientSecret, DateTime.UtcNow.ToString("yyyyMMddHHmm"))
                .ToMd5();

            // 若请求头中的签名与本地计算一致，则跳过权限校验
            if (unChecked.ToString() == sign)
                return;
        }

        // 收集当前 Action 方法及其所属 Controller 上的所有自定义特性
        var attrs = new List<object>();
        var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        attrs.AddRange(controllerActionDescriptor.MethodInfo.GetCustomAttributes(true));
        attrs.AddRange(controllerActionDescriptor.MethodInfo.DeclaringType.GetCustomAttributes(true));

        // 筛选出所有 [CheckPermission] 特性
        var checkPermAttrs = attrs.OfType<CheckPermissionAttribute>().ToList();

        // 获取当前用户的声明（Claims），虽然此处未直接使用，但可能用于后续扩展
        var claims = context.HttpContext.User.Claims;

        // 如果存在 [CheckPermission] 特性，则进行权限校验
        if (checkPermAttrs.Count > 0)
        {
            // 默认从路由中获取 controller 和 action 名称
            var controller = context.ActionDescriptor.RouteValues["controller"];
            var action = context.ActionDescriptor.RouteValues["action"];

            // 如果特性中指定了 Controller 或 Action 名称，则优先使用特性中的值
            if (!checkPermAttrs.First().Controller.IsNullOrEmpty())
                controller = checkPermAttrs.First().Controller;
            if (!checkPermAttrs.First().Action.IsNullOrEmpty())
                action = checkPermAttrs.First().Action;

            // 调用远程权限服务进行校验（同步等待结果）
            var msg = _httpPermService.ValidateAuthorization(controller, action).Result;

            // 如果校验成功，继续执行后续操作
            if (msg.ErrType.Equals(BaseErrType.Success))
            {
                return;
            }
            else
            {
                // 校验失败，返回错误信息作为 JSON 结果，中断请求
                context.Result = new JsonResult(msg);
            }
        }
    }
}

/// <summary>
/// 权限检测特性。
/// 应用于控制器方法（或类）上，标记该方法需要进行权限校验。
/// 可指定自定义的 Controller 和 Action 名称（用于权限系统识别）。
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class CheckPermissionAttribute : AuthorizeAttribute
{
    /// <summary>
    /// 可选：覆盖默认的控制器名称，用于权限系统匹配。
    /// </summary>
    public string Controller { get; set; }

    /// <summary>
    /// 可选：覆盖默认的操作方法名称，用于权限系统匹配。
    /// </summary>
    public string Action { get; set; }
}