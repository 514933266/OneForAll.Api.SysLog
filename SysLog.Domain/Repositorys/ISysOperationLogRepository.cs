using System.Threading.Tasks;
using OneForAll.Core;
using OneForAll.EFCore;
using SysLog.Domain.AggregateRoots;

namespace SysLog.Domain.Repositorys
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public interface ISysOperationLogRepository : IEFCoreRepository<SysOperationLog>
    {
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字：系统名称 模块名称</param>
        ///  <returns>分页</returns>
        Task<PageList<SysOperationLog>> GetPageAsync(int pageIndex, int pageSize, string key);
    }
}
