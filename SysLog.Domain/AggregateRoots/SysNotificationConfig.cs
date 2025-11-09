using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysLog.Domain.Enums;

namespace SysLog.Domain.AggregateRoots
{
    /// <summary>
    /// 通知设置
    /// </summary>
    public class SysNotificationConfig
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        [Required]
        public SysLogTypeEnum LogType { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        [Required]
        public SysNotificationTypeEnum NotificationType { get; set; }

        /// <summary>
        /// 目标值：[]
        /// </summary>
        [Required]
        public string TargetJson { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; } = true;
    }
}
