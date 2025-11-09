using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using OneForAll.Core;
using OneForAll.Core.Extension;
using Org.BouncyCastle.Asn1.Cms;
using SysLog.Domain.AggregateRoots;
using SysLog.Domain.Aggregates;
using SysLog.Domain.Interfaces;
using SysLog.Domain.Repositorys;
using SysLog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain
{
    /// <summary>
    /// 活跃度
    /// </summary>
    public class LivenessManager : SysBaseManager, ILivenessManager
    {
        private readonly string CACHE_KEY = "Liveness_{0}:{1}";

        private readonly IDistributedCache _cacheRepository;
        private readonly ISysApiLogRepository _repository;

        public LivenessManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache cacheRepository,
            ISysApiLogRepository repository) : base(mapper, httpContextAccessor)
        {
            _repository = repository;
            _cacheRepository = cacheRepository;
        }

        /// <summary>
		/// 查询当前登录用户活跃指数
		/// </summary>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		///  <returns>分页</returns>
		public async Task<UserLivenessAggr> GetAsync(DateTime startTime, DateTime endTime)
        {
            var day = (endTime.Date - startTime.Date).Days;
            var cacheKey = CACHE_KEY.Fmt(day, LoginUser.Id);
            var result = new UserLivenessAggr() { UserId = LoginUser.Id, StartDate = startTime, EndDate = endTime };
            try
            {
                result.InitDate(startTime, endTime);
                var cache = await _cacheRepository.GetStringAsync(cacheKey);
                if (cache.IsNullOrEmpty())
                {
                    // 没有缓存，直接读库
                    var data = await _repository.GetListAsync(LoginUser.SysTenantId.ToString(), startTime, endTime);
                    result.CalculateDateScope(data);
                    await _cacheRepository.SetStringAsync(cacheKey, result.ToJson(), new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(10) });
                }
                else
                {
                    result = cache.FromJson<UserLivenessAggr>();
                }
            }
            catch
            {
                var data = await _repository.GetListAsync(LoginUser.SysTenantId.ToString(), startTime, endTime);
                result.CalculateDateScope(data);
            }

            return result;
        }
    }
}
