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
        /// 基础信息服务
        /// </summary>
        public string SysBase { get; set; } = "SysBase";

        /// <summary>
        /// 消息服务
        /// </summary>
        public string SysUms { get; set; } = "SysUms";

        /// <summary>
        /// 定时任务调度服务
        /// </summary>
        public string SysJob { get; set; } = "SysJob";
    }
}
