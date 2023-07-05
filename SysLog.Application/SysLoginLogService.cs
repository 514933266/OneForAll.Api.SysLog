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
	/// 登录日志
	/// </summary>
	public class SysLoginLogService : ISysLoginLogService
	{
		private readonly IMapper _mapper;
		private readonly ISysLoginLogManager _manager;
		private readonly ISysLoginLogRepository _repository;
		public SysLoginLogService(
			IMapper mapper,
			ISysLoginLogManager manager,
			ISysLoginLogRepository repository)
		{
			_mapper = mapper;
			_manager = manager;
			_repository = repository;
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
		public async Task<PageList<SysLoginLogDto>> GetPgaeAsync(
			int pageIndex,
			int pageSize,
			DateTime? startTime,
			DateTime? endTime,
			string userName,
			string key)
		{
			var data = await _manager.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, userName, key);
			var items = _mapper.Map<IEnumerable<SysLoginLog>, IEnumerable<SysLoginLogDto>>(data.Items);
			return new PageList<SysLoginLogDto>(data.Total, data.PageIndex, data.PageSize, items);
		}

		/// <summary>
		/// 添加
		/// </summary>
		/// <param name="entity">实体</param>
		/// <returns>结果</returns>
		public async Task<BaseErrType> AddAsync(SysLoginLogForm entity)
		{
			return await _manager.AddAsync(entity);
		}
	}
}
