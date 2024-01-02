using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain.Enums
{
    /// <summary>
    /// 消息通知类型
    /// </summary>
    public enum SysNotificationTypeEnum
    {
        /// <summary>
        /// 企业微信机器人
        /// </summary>
        WxQtRoot = 1000,

        /// <summary>
        /// 微信公众号
        /// </summary>
        Wxgzh = 1001,

        /// <summary>
        /// 短信
        /// </summary>
        Sms = 2000,

        /// <summary>
        /// 邮件
        /// </summary>
        Email = 3000
    }
}
