using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneForAll.Core;
using Microsoft.AspNetCore.Authorization;

namespace SysLog.Host.Models
{
    /// <summary>
    /// 权限检查特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckPermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 权限
        /// </summary>
        public string Permission { get; set; }
    }
}