using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using OneForAll.Core.DDD;

namespace SysLog.Domain.AggregateRoots
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class SysOperationLog
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
        public Guid SysTenantId { get; set; }

        /// <summary>
        /// 所属模块名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MoudleName { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MoudleCode { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Controller { get; set; }

        /// <summary>
        /// 控制器方法
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Action { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        /// <summary>
        /// 详细内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(300)]
        public string Remark { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        [Required]
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
