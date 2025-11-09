using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using SysLog.Domain.AggregateRoots;
using SysLog.Domain.Interfaces;
using SysLog.Domain.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain
{
    /// <summary>
    /// 企业活动
    /// </summary>
    public class ActivityManager : SysBaseManager, IActivityManager
    {
        private readonly ISysLoginLogRepository _loginLogRepository;

        public ActivityManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysLoginLogRepository loginLogRepository) : base(mapper, httpContextAccessor)
        {
            _loginLogRepository = loginLogRepository;
        }

        /// <summary>
		/// 查询登录列表
		/// </summary>
		///  <returns>活跃度</returns>
        public async Task<IEnumerable<SysLoginLog>> GetListLoginTodayAsync(int top)
        {
            return await _loginLogRepository.GetListTodayAsync(LoginUser.SysTenantId.ToString(), top);
        }
    }
}
