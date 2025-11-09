using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.HttpService.Interfaces
{
    /// <summary>
    /// 消息通知
    /// </summary>
    public interface ISysUmsMessageHttpService
    {
        /// <summary>
        /// 企业微信机器人通知：Mardown
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns></returns>
        Task SendToWxqyRobotMarkdownAsync(UmsWxqyRobotTextForm form);
    }
}
