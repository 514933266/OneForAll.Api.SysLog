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
    /// 全局异常日志
    /// </summary>
    public class SysGlobalExceptionLog
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

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
        /// 异常名称
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        /// <summary>
        /// 详细内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    }
}
