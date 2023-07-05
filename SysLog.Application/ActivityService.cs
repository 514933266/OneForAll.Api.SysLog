using AutoMapper;
using SysLog.Application.Dtos;
using SysLog.Application.Interfaces;
using SysLog.Domain;
using SysLog.Domain.AggregateRoots;
using SysLog.Domain.Aggregates;
using SysLog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Application
{
    /// <summary>
    /// 企业活动
    /// </summary>
    public class ActivityService : IActivityService
    {
        private readonly IMapper _mapper;
        private readonly IActivityManager _manager;
        public ActivityService(IMapper mapper, IActivityManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
		/// 查询登录列表
		/// </summary>
		///  <returns>列表</returns>
        public async Task<IEnumerable<SysLoginLogDto>> GetListLoginAsync(int top)
        {
            var data = await _manager.GetListLoginTodayAsync(top);
            return _mapper.Map<IEnumerable<SysLoginLog>, IEnumerable<SysLoginLogDto>>(data);
        }
    }
}
