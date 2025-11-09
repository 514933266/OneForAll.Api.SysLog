using AutoMapper;
using Microsoft.Extensions.Configuration;
using OneForAll.Core;
using SysLog.Application.Dtos;
using SysLog.Application.Interfaces;
using SysLog.Domain.AggregateRoots;
using SysLog.Domain.Interfaces;
using SysLog.Domain.Models;
using SysLog.HttpService.Interfaces;
using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SysLog.Application
{
	/// <summary>
	/// 全局异常
	/// </summary>
	public class SysGlobalExceptionLogService : ISysGlobalExceptionLogService
	{
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;
		private readonly ISysGlobalExceptionLogManager _manager;
		private readonly ISysUmsMessageHttpService _umsHttpService;
		public SysGlobalExceptionLogService(
			IMapper mapper,
			IConfiguration configuration,
			ISysGlobalExceptionLogManager manager,
			ISysUmsMessageHttpService umsHttpService)
		{
			_mapper = mapper;
			_configuration = configuration;
			_manager = manager;
			_umsHttpService = umsHttpService;
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
		public async Task<PageList<SysGlobalExceptionLogDto>> GetPgaeAsync(
			int pageIndex,
			int pageSize,
			DateTime? startTime,
			DateTime? endTime,
			string userName,
			string key)
		{
			var data = await _manager.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, userName, key);
			var items = _mapper.Map<IEnumerable<SysGlobalExceptionLog>, IEnumerable<SysGlobalExceptionLogDto>>(data.Items);
			return new PageList<SysGlobalExceptionLogDto>(data.Total, data.PageIndex, data.PageSize, items);
		}

		/// <summary>
		/// 添加
		/// </summary>
		/// <param name="form">实体</param>
		/// <returns>结果</returns>
		public async Task<BaseErrType> AddAsync(SysGlobalExceptionLogForm form)
		{
			var webhookUrl = _configuration["HttpService:WebhookUrl"];
			if (!string.IsNullOrEmpty(webhookUrl))
			{
				var time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
				var content = $@"
					## ⚠️ <font color=""red"">警告：服务引发全局异常</font>
					> **模块**：{form.ModuleName}  
					> **代码**：`{form.ModuleCode}`  
					> **异常摘要**：{form.Name}  
					> **发生时间**：{time}
					---
					请尽快排查处理！";

				await _umsHttpService.SendToWxqyRobotMarkdownAsync(new UmsWxqyRobotTextForm()
				{
					Content = content,
					WebhookUrl = webhookUrl
				});
			}
			return await _manager.AddAsync(form);
		}
	}
}
