using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using OneForAll.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SysLog.Domain.Models;
using SysLog.Public.Models;
using SysLog.Application.Dtos;
using SysLog.Application.Interfaces;
using SysLog.Host.Filters;
using OneForAll.Core.OAuth;

namespace SysLog.Host.Controllers
{
	/// <summary>
	/// 异常日志
	/// </summary>
	[Route("api/[controller]")]
	public class SysExceptionLogsController : BaseController
	{
		private readonly ISysExceptionLogService _service;

		public SysExceptionLogsController(ISysExceptionLogService service)
		{
			_service = service;
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
		[HttpGet]
		[Route("{pageIndex}/{pageSize}")]
		[CheckPermission(Action = ConstPermission.EnterView)]
		public async Task<PageList<SysExceptionLogDto>> GetPgaeAsync(
			int pageIndex,
			int pageSize,
			[FromQuery] DateTime? startTime,
			[FromQuery] DateTime? endTime,
			[FromQuery] string userName = default,
			[FromQuery] string controller = default,
			[FromQuery] string action = default,
			[FromQuery] string key = default)
		{
			return await _service.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, userName, controller, action, key);
		}

		/// <summary>
		/// 添加
		/// </summary>
		[HttpPost]
		public async Task<BaseMessage> AddAsync([FromBody] SysExceptionLogForm entity)
		{
			var msg = new BaseMessage();
			msg.ErrType = await _service.AddAsync(entity);

			switch (msg.ErrType)
			{
				case BaseErrType.Success: return msg.Success("添加成功");
				case BaseErrType.NotAllow: return msg.Success("暂不支持记录日志");
				default: return msg.Fail("添加失败");
			}
		}
	}
}
