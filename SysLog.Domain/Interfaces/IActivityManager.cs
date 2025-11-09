using SysLog.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain.Interfaces
{
    /// <summary>
    /// 企业活动
    /// </summary>
    public interface IActivityManager
    {
        /// <summary>
		/// 查询登录列表
		/// </summary>
		///  <returns>活跃度</returns>
        Task<IEnumerable<SysLoginLog>> GetListLoginTodayAsync(int top);
    }
}
