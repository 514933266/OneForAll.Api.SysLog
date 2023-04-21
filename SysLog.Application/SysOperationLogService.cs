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
	/// 系统操作日志
	/// </summary>
	public class SysOperationLogService : ISysOperationLogService
	{
		private readonly IMapper _mapper;
		private readonly ISysOperationLogManager _manager;

		public SysOperationLogService(
			IMapper mapper,
			ISysOperationLogManager manager)
		{
			_mapper = mapper;
			_manager = manager;
		}

		/// <summary>
		/// 查询分页
		/// </summary>
		/// <param name="pageIndex">页码</param>
		/// <param name="pageSize">页数</param>
		/// <param name="key">关键字：系统名称 模块名称</param>
		public async Task<PageList<SysOperationLogDto>> GetPgaeAsync(int pageIndex, int pageSize, string key)
		{
			var data = await _manager.GetPgaeAsync(pageIndex, pageSize, key);
			var items = _mapper.Map<IEnumerable<SysOperationLog>, IEnumerable<SysOperationLogDto>>(data.Items);
			return new PageList<SysOperationLogDto>(data.Total, data.PageIndex, data.PageSize, items);
		}

		/// <summary>
		/// 添加
		/// </summary>
		/// <param name="entity">实体</param>
		/// <returns>结果</returns>
		public async Task<BaseErrType> AddAsync(SysOperationLogForm entity)
		{
			return await _manager.AddAsync(entity);
		}
	}
}
