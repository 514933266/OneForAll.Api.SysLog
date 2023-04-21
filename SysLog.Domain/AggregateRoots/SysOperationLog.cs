using System;
using System.ComponentModel.DataAnnotations;
using OneForAll.Core.DDD;

namespace SysLog.Domain.AggregateRoots
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class SysOperationLog : AggregateRoot<Guid>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public Guid SysTenantId { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string SystemName { get; set; }

        /// <summary>
        /// 系统代码，OneForAll.OA
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string SystemCode { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块代码，OAPerson
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ModuleCode { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Type { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        [Required]
        public int Grade { get; set; }

        /// <summary>
        /// 详细内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(300)]
        public string Remark { get; set; }

        /// <summary>
        /// 操作人id
        /// </summary>
        [Required]
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }
    }
}
