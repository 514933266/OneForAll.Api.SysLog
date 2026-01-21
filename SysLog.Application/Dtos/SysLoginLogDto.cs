using System;

namespace SysLog.Application.Dtos
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class SysLoginLogDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string  UserName { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 登录方式
        /// </summary>
        public string LoginType { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
