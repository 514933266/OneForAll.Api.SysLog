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
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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

		private string GetWechatQyRobotTextContent(SysExceptionLogForm form)
		{
			var sb = new StringBuilder("## <font color=\"red\">警告：服务引发异常</font>  \r\n");
			sb.Append($"模块：{form.MoudleName}  \r\n");
			sb.Append($"代码：{form.MoudleCode}  \r\n");
			sb.Append($"控制器：{form.Controller}  \r\n");
			sb.Append($"方法：{form.Action}  \r\n");
			sb.Append($"异常摘要：{form.Name}  \r\n");
			sb.Append($"发生时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  \r\n");
			return sb.ToString();
		}
	}
}
