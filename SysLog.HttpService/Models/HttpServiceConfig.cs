using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysLog.HttpService.Models
{
    /// <summary>
    /// 数据资源服务配置
    /// </summary>
    public class HttpServiceConfig
    {
        /// <summary>
        /// 权限验证接口
        /// </summary>
        public string SysPermissionCheck { get; set; } = "SysPermissionCheck";
    }
}
