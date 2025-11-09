using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace SysLog.Application.Dtos
{
    /// <summary>
    /// 异常日志
    /// </summary>
    public class SysExceptionLogDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 所属模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 控制器方法
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 详细内容
        /// </summary>
        public string Content { get; set; }

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
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

    }
}
