using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneForAll.Core;
using OneForAll.EFCore;
using SysLog.Domain.AggregateRoots;
using SysLog.Domain.ValueObjects;

namespace SysLog.Domain.Repositorys
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public interface ISysLoginLogRepository : IEFCoreRepository<SysLoginLog>
    {
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="userName">操作人</param>
        /// <param name="key">关键字</param>
        ///  <returns>分页</returns>
        Task<PageList<SysLoginLog>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string userName,
            string key);

        /// <summary>
		/// 查询机构登录记录列表
		/// </summary>
		/// <param name="tenantId">租户Id</param>
		/// <param name="top">数据量</param>
		///  <returns>登录列表</returns>
		Task<IEnumerable<SysLoginLog>> GetListTodayAsync(string tenantId, int top);
    }
}
