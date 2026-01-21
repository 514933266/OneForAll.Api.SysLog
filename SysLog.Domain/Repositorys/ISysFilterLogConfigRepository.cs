using OneForAll.EFCore;
using SysLog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain.Repositorys
{
    /// <summary>
    /// 日志过滤配置
    /// </summary>
    public interface ISysFilterLogConfigRepository : IEFCoreRepository<SysFilterLogConfig>
    {
        /// <summary>
        /// 获取缓存（当不存在缓存时返回数据库值）
        /// </summary>
        /// <returns></returns>
        Task<HashSet<string>> GetListCacheAsync();
    }
}
