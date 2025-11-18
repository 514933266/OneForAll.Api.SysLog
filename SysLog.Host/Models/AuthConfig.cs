using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysLog.Host.Models
{
    /// <summary>
    /// OAuth授权配置
    /// </summary>
    public class AuthConfig
    {
        /// <summary>
        /// 客户端id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端密码
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 客户端代码
        /// </summary>
        public string ClientCode { get; set; }

        /// <summary>
        /// 授权服务地址
        /// </summary>
        public string Authority { get; set; }

    }
}
