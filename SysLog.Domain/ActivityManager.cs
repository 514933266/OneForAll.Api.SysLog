using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using SysLog.Domain.Entities;
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
    public class ActivityManager : BaseManager, IActivityManager
    {
        private readonly IMapper _mapper;
        private readonly ISysLoginLogRepository _loginLogRepository;

        public ActivityManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysLoginLogRepository loginLogRepository) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _loginLogRepository = loginLogRepository;
        }

        /// <summary>
		/// 查询登录列表
		/// </summary>
		///  <returns>活跃度</returns>
        public async Task<IEnumerable<SysLoginLog>> GetListLoginTodayAsync(int top)
        {
            return await _loginLogRepository.GetListTodayAsync(LoginUser.TenantId.ToString(), top);
        }
    }
}
