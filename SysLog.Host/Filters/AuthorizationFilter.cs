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

namespace SysLog.Host.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly AuthConfig _config;
        private readonly ISysPermissionCheckHttpService _httpPermService;
        public AuthorizationFilter(
            AuthConfig config,
            ISysPermissionCheckHttpService httpPermService)
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
            var attrs = new List<object>();
            attrs.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true));
            attrs.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.DeclaringType.GetCustomAttributes(true));
            var checkPermAttrs = attrs.OfType<CheckPermissionAttribute>().ToList();
            var claims = context.HttpContext.User.Claims;
            if (checkPermAttrs.Count > 0)
            {
                var controller = context.ActionDescriptor.RouteValues["controller"];
                var action = context.ActionDescriptor.RouteValues["action"];
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
    }
}