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
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Application
{
	/// <summary>
	/// 异常日志
	/// </summary>
	public class SysExceptionLogService : ISysExceptionLogService
	{
		private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ISysExceptionLogManager _manager;
		private readonly ISysUmsMessageHttpService _umsHttpService;
		public SysExceptionLogService(
			IMapper mapper,
            IConfiguration configuration,
            ISysExceptionLogManager manager,
			ISysUmsMessageHttpService umsHttpService)
		{
			_mapper = mapper;
			_manager = manager;
			_configuration = configuration;
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
		/// <param name="controller">控制器</param>
		/// <param name="action">方法</param>
		/// <param name="key">关键字</param>
		///  <returns>分页</returns>
		public async Task<PageList<SysExceptionLogDto>> GetPgaeAsync(
			int pageIndex,
			int pageSize,
			DateTime? startTime,
			DateTime? endTime,
			string userName,
			string controller,
			string action,
			string key)
		{
			var data = await _manager.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, userName, controller, action, key);
			var items = _mapper.Map<IEnumerable<SysExceptionLog>, IEnumerable<SysExceptionLogDto>>(data.Items);
			return new PageList<SysExceptionLogDto>(data.Total, data.PageIndex, data.PageSize, items);
		}

		/// <summary>
		/// 添加
		/// </summary>
		/// <param name="form">实体</param>
		/// <returns>结果</returns>
		public async Task<BaseErrType> AddAsync(SysExceptionLogForm form)
		{
            var webhookUrl = _configuration["HttpService:WebhookUrl"];
            if (!string.IsNullOrEmpty(webhookUrl))
			{
				var time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
				var content = $@"
					## ⚠️ <font color=""red"">警告：服务引发异常</font>
					> **模块**：{form.ModuleName}  
					> **代码**：`{form.ModuleCode}`  
					> **控制器**：`{form.Controller}`  
					> **方法**：`{form.Action}`  
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
