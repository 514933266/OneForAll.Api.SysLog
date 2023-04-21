using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using OneForAll.Core;
using SysLog.Domain.Models;
using SysLog.Application.Dtos;
using SysLog.Domain.Interfaces;
using SysLog.Domain.Repositorys;
using SysLog.Domain.AggregateRoots;
using SysLog.Application.Interfaces;

namespace SysLog.Application
{
	/// <summary>
	/// 系统日志
	/// </summary>
	public class SysApiLogService : ISysApiLogService
	{
		private readonly IMapper _mapper;
		private readonly ISysApiLogManager _manager;
		private readonly ISysApiLogRepository _repository;
		public SysApiLogService(
			IMapper mapper,
			ISysApiLogManager sysLogManager,
			ISysApiLogRepository sysLogRepository)
		{
			_mapper = mapper;
			_manager = sysLogManager;
			_repository = sysLogRepository;
		}

		/// <summary>
		/// 查询分页
		/// </summary>
		/// <param name="pageIndex">页码</param>
		/// <param name="pageSize">页数</param>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		/// <param name="userName">操作人</param>
		/// <param name="key">关键字</param>
		///  <returns>分页</returns>
		public async Task<PageList<SysApiLogDto>> GetPgaeAsync(
			int pageIndex,
			int pageSize,
			DateTime? startTime,
			DateTime? endTime,
			string userName,
			string key)
		{
			var data = await _manager.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, userName, key);
			var items = _mapper.Map<IEnumerable<SysApiLog>, IEnumerable<SysApiLogDto>>(data.Items);
			return new PageList<SysApiLogDto>(data.Total, data.PageIndex, data.PageSize, items);
		}

		/// <summary>
		/// 添加
		/// </summary>
		/// <param name="entity">实体</param>
		/// <returns>结果</returns>
		public async Task<BaseErrType> AddAsync(SysApiLogForm entity)
		{
			return await _manager.AddAsync(entity);
		}
	}
}
