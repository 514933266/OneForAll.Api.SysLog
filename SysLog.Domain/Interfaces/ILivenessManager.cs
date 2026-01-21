using SysLog.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain.Interfaces
{
    /// <summary>
    /// 活跃度
    /// </summary>
    public interface ILivenessManager
    {
        /// <summary>
		/// 查询当前登录用户活跃指数
		/// </summary>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		///  <returns>分页</returns>
		Task<UserLivenessAggr> GetAsync(DateTime startTime, DateTime endTime);
    }
}
