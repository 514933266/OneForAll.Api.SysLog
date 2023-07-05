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
    /// 全局异常
    /// </summary>
    public interface ISysGlobalExceptionLogRepository : IEFCoreRepository<SysGlobalExceptionLog>
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
        Task<PageList<SysGlobalExceptionLog>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string userName,
            string key);
    }
}
