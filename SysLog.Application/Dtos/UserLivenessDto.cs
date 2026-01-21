using SysLog.Domain.Aggregates;
using SysLog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Application.Dtos
{
    /// <summary>
    /// 用户活跃度
    /// </summary>
    public class UserLivenessDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 每日活跃度
        /// </summary>
        public List<UserLivenessDateVo> Dates { get; set; } = new List<UserLivenessDateVo>();
    }
}
