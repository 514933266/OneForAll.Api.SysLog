using System;

namespace SysLog.Application.Dtos
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class SysOperationLogDto
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 系统代码
        /// </summary>
        public string SystemCode { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 操作人id
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 详细内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
