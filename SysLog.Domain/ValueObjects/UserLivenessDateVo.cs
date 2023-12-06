using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain.ValueObjects
{
    /// <summary>
    /// 用户活跃度日期
    /// </summary>
    public class UserLivenessDateVo
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 平均数值
        /// </summary>
        public int AvgValue { get; set; }
    }
}
