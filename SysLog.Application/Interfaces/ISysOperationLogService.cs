using System;
using System.Threading.Tasks;
using OneForAll.Core;
using SysLog.Domain.Models;
using SysLog.Application.Dtos;

namespace SysLog.Application.Interfaces
{
    /// <summary>
    /// 服务接口：系统操作日志
    /// </summary>
    public interface ISysOperationLogService
    {
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字：系统名称 模块名称</param>
        Task<PageList<SysOperationLogDto>> GetPgaeAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(SysOperationLogForm entity);
    }
}
