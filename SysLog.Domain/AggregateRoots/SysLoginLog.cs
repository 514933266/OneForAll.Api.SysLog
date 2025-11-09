using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain.AggregateRoots
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class SysLoginLog
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public string SysTenantId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string  UserName { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Source { get; set; }

        /// <summary>
        /// 登录方式
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string LoginType { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string IPAddress { get; set; }

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
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    }
}
