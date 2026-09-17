using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SysLog.Application.Dtos;
using SysLog.Application.Interfaces;
using OneForAll.Core.Extension;
using OneForAll.Core.OAuth;

namespace SysLog.Host.Controllers
{
    /// <summary>
	/// 活跃度
	/// </summary>
	[Route("api/[controller]")]
    [AllowAnonymous]
    public class LivenessController : BaseController
    {
        private readonly ILivenessService _service;

        public LivenessController(ILivenessService service)
        {
            _service = service;
        }

        /// <summary>
		/// 查询当前登录用户的活跃度
		/// </summary>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		///  <returns>活跃度</returns>
		[HttpGet]
        public async Task<UserLivenessDto> GetAsync([FromQuery] DateTime? startTime, [FromQuery] DateTime? endTime)
        {
            var startDate = startTime ?? DateTime.UtcNow.AddDays(-3);
            var endDate = endTime ?? DateTime.UtcNow;
            // 未登录（管理界面免登录访问）时返回空活跃度
            if (LoginUser.Id.IsNullOrEmpty())
                return new UserLivenessDto() { StartDate = startDate, EndDate = endDate };
            return await _service.GetAsync(startDate, endDate);
        }
    }
}
