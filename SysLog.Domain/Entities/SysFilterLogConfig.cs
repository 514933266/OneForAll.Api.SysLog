using SysLog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain.Entities
{
    /// <summary>
    /// 日志过滤配置
    /// </summary>
    public class SysFilterLogConfig
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        [Required]
        public SysLogTypeEnum LogType { get; set; }

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
        /// 控制器方法
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Method { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
    }
}
