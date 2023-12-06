using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using OneForAll.Core;
using SysLog.Application.Dtos;
using SysLog.Domain.Models;

namespace SysLog.Application.Interfaces
{
    /// <summary>
    /// 服务接口：系统日志
    /// </summary>
    public interface ISysApiLogService
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
        Task<PageList<SysApiLogDto>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string userName,
            string controller,
            string action,
            string key);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(SysApiLogForm entity);
    }
}
