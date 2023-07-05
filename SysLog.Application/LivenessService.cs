using AutoMapper;
using SysLog.Application.Dtos;
using SysLog.Application.Interfaces;
using SysLog.Domain.AggregateRoots;
using SysLog.Domain.Aggregates;
using SysLog.Domain.Interfaces;
using SysLog.Domain.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Application
{
    /// <summary>
    /// 活跃度
    /// </summary>
    public class LivenessService : ILivenessService
    {
        private readonly IMapper _mapper;
        private readonly ILivenessManager _manager;
        public LivenessService(
            IMapper mapper,
            ILivenessManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
		/// 查询当前登录用户活跃指数
		/// </summary>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		///  <returns>分页</returns>
		public async Task<UserLivenessDto> GetAsync(DateTime startTime, DateTime endTime)
        {
            var data = await _manager.GetAsync(startTime, endTime);
            return _mapper.Map<UserLivenessAggr, UserLivenessDto>(data);
        }
    }
}
