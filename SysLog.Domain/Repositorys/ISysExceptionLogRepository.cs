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
    /// 异常日志
    /// </summary>
    public interface ISysExceptionLogRepository : IEFCoreRepository<SysExceptionLog>
    {
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="userName">操作人</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">方法</param>
        /// <param name="key">关键字</param>
        ///  <returns>分页</returns>
        Task<PageList<SysExceptionLog>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string userName,
            string controller,
            string action,
            string key);
    }
}
