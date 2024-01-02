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
using SysLog.HttpService.Interfaces;
using SysLog.HttpService.Models;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Text;
using OneForAll.Core.Extension;

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
			var url = _configuration["HttpService:UmsWechatQyRobotWebhookUrl"];
			if (!url.IsNullOrEmpty())
			{
				var content = GetWechatQyRobotTextContent(form);

				await _umsHttpService.SendToWechatQyRobotMarkdownAsync(new UmsWechatQyRobotTextForm()
				{
					Content = content,
					WebhookUrl = url
				});
			}
			return await _manager.AddAsync(form);
		}

		private string GetWechatQyRobotTextContent(SysGlobalExceptionLogForm form)
		{
			var sb = new StringBuilder("## <font color=\"red\">警告：服务引发全局异常</font>  \r\n");
			sb.Append($"模块：{form.MoudleName}  \r\n");
			sb.Append($"代码：{form.MoudleCode}  \r\n");
			sb.Append($"异常摘要：{form.Name}  \r\n");
			sb.Append($"发生时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  \r\n");
			return sb.ToString();
		}
	}
}
