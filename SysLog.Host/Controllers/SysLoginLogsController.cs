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

namespace SysLog.Host.Controllers
{
	/// <summary>
	/// 登录日志
	/// </summary>
	[Route("api/[controller]")]
	public class SysLoginLogsController : BaseController
	{
		private readonly ISysLoginLogService _service;

		public SysLoginLogsController(ISysLoginLogService service)
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
		/// <param name="key">关键字</param>
		///  <returns>分页</returns>
		[HttpGet]
		[Route("{pageIndex}/{pageSize}")]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<PageList<SysLoginLogDto>> GetPgaeAsync(
			int pageIndex,
			int pageSize,
			[FromQuery] DateTime? startTime,
			[FromQuery] DateTime? endTime,
			[FromQuery] string userName,
			[FromQuery] string key)
		{
			return await _service.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, userName, key);
		}

		/// <summary>
		/// 添加
		/// </summary>
		[HttpPost]
        public async Task<BaseMessage> AddAsync([FromBody] SysLoginLogForm entity)
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
