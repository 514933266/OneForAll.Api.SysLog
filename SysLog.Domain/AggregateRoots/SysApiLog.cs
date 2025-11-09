using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using OneForAll.Core.DDD;

namespace SysLog.Domain.AggregateRoots
{
    /// <summary>
    /// API日志
    /// </summary>
    public class SysApiLog
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public string SysTenantId { get; set; }

        /// <summary>
        /// 所属模块名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ModuleCode { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Controller { get; set; }

        /// <summary>
        /// 控制器方法
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Action { get; set; }

        /// <summary>
        /// 请求域名
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Host { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        [Required]
        [MaxLength(1000)]
        public string Url { get; set; }

        /// <summary>
        /// 请求方法：GET、POST、PUT、DELETE等
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Method { get; set; }

        /// <summary>
        /// 请求类型，如果是文件类型
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ContentType { get; set; }

        /// <summary>
        /// 请求Body内容，如果是文件上传类不记录
        /// </summary>
        [Required]
        public string RequestBody { get; set; } = "";

        /// <summary>
        /// 响应内容，如果是下载不记录
        /// </summary>
        [Required]
        public string ReponseBody { get; set; } = "";

        /// <summary>
        /// 完整的浏览器信息
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string UserAgent { get; set; } = "";

        /// <summary>
        /// Ip地址
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string IPAddress { get; set; }

        /// <summary>
        /// 消耗时间
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string TimeSpan { get; set; } = "00:00:00";

        /// <summary>
        /// 状态码
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string StatusCode { get; set; } = "200";

        /// <summary>
        /// 创建人id
        /// </summary>
        [Required]
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string CreatorName { get; set; } = "系统用户";

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

    }
}
