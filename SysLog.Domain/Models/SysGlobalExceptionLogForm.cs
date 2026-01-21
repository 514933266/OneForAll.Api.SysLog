using System;
using System.ComponentModel.DataAnnotations;

namespace SysLog.Domain.Models
{
    /// <summary>
    /// 全局异常
    /// </summary>
    public class SysGlobalExceptionLogForm
    {
        /// <summary>
        /// 所属模块名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ModuleCode { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        /// <summary>
        /// 详细内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
