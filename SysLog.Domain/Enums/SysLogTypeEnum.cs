using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain.Enums
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum SysLogTypeEnum
    {
        /// <summary>
        /// Api日志
        /// </summary>
        Api = 1000,

        /// <summary>
        /// 异常日志
        /// </summary>
        Exception = 2000,

        /// <summary>
        /// 全局异常日志
        /// </summary>
        GlobalException = 3000,

        /// <summary>
        /// 登录日志
        /// </summary>
        Login = 4000,

        /// <summary>
        /// 操作日志
        /// </summary>
        Operation = 5000
    }
}
