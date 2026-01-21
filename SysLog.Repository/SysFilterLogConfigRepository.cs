using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneForAll.Core.Extension;
using OneForAll.Core.ORM;
using OneForAll.EFCore;
using SysLog.Domain.Entities;
using SysLog.Domain.Enums;
using SysLog.Domain.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SysLog.Repository
{
    /// <summary>
    /// 日志过滤配置
    /// </summary>
    public class SysFilterLogConfigRepository : Repository<SysFilterLogConfig>, ISysFilterLogConfigRepository
    {
        private readonly IDistributedCache _cache;
        public SysFilterLogConfigRepository(DbContext context, IDistributedCache cache) : base(context)
        {
            _cache = cache;
        }

        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 获取缓存（当不存在缓存时返回数据库值）
        /// </summary>
        /// <returns></returns>
        public async Task<HashSet<string>> GetListCacheAsync()
        {
            var key = "Sys:FilterLogConfig";
            var cached = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cached))
            {
                return cached.FromJson<HashSet<string>>();
            }

            // 加锁加载（参考前文）
            await _lock.WaitAsync();
            try
            {
                cached = await _cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(cached))
                    return cached.FromJson<HashSet<string>>();

                var list = (await GetListAsync()).ToList();
                var keySet = new HashSet<string>(
                    list.Where(x => x.LogType == SysLogTypeEnum.Api)
                        .Select(x => $"{x.ModuleCode}|{x.Controller}|{x.Action}|{x.Method}".ToLowerInvariant())
                );

                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                await _cache.SetStringAsync(key, keySet.ToJson(), options);
                return keySet;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
