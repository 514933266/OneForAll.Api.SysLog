using OneForAll.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using SysLog.Domain;
using SysLog.Host.Models;
using SysLog.Domain.Interfaces;
using SysLog.HttpService.Interfaces;
using Base.Host.Models;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using Microsoft.AspNetCore.Authorization;

namespace SysLog.Host.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly AuthConfig _config;
        private readonly ISysPermissionCheckHttpService _httpPermService;
        public AuthorizationFilter(AuthConfig config, ISysPermissionCheckHttpService httpPermService)
        {
            _config = config;
            _httpPermService = httpPermService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter) ||
                !(context.ActionDescriptor is ControllerActionDescriptor))
            {
                return;
            }
            var unChecked = context.HttpContext.Request.Headers["Unchecked"];
            if (!unChecked.IsNull())
            {
                // 不检查权限
                var sign = "clientId={0}&clientSecret={1}&apiName={2}&tt={3}".Fmt(_config.ClientId, _config.ClientSecret, _config.ApiName, DateTime.Now.ToString("yyyyMMddhhmm")).ToMd5();
                if (unChecked.ToString() == sign) return;
            };

            var attrs = new List<object>();
            attrs.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true));
            attrs.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.DeclaringType.GetCustomAttributes(true));
            var checkPermAttrs = attrs.OfType<CheckPermissionAttribute>().ToList();
            var claims = context.HttpContext.User.Claims;
            if (checkPermAttrs.Count > 0)
            {
                var controller = context.ActionDescriptor.RouteValues["controller"];
                var action = context.ActionDescriptor.RouteValues["action"];
                if (!checkPermAttrs.First().Controller.IsNullOrEmpty()) controller = checkPermAttrs.First().Controller;
                if (!checkPermAttrs.First().Action.IsNullOrEmpty()) action = checkPermAttrs.First().Action;
                var msg = _httpPermService.ValidateAuthorization(controller, action).Result;
                if (msg.ErrType.Equals(BaseErrType.Success))
                {
                    return;
                }
                else
                {
                    context.Result = new JsonResult(msg);
                }
            }
        }

        // 校验功能权限
        private BaseMessage ValidateAuthorization(AuthorizationFilterContext context, List<CheckPermissionAttribute> attrs)
        {
            var msg = new BaseMessage();
            var controller = context.ActionDescriptor.RouteValues["controller"];
            var action = context.ActionDescriptor.RouteValues["action"];

            foreach (var attr in attrs)
            {
                controller = attr.Controller.IsNullOrEmpty() ? controller : attr.Controller;
                action = attr.Action.IsNullOrEmpty() ? action : attr.Action;
                msg = _httpPermService.ValidateAuthorization(controller, action).Result;
                if (msg.ErrType == BaseErrType.Success)
                    break;
            }
            return msg;
        }
    }

    /// <summary>
    /// 权限检测
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckPermissionAttribute : AuthorizeAttribute
    {
        public string Controller { get; set; }
        public string Action { get; set; }

    }
}