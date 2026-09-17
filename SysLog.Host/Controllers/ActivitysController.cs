using Microsoft.AspNetCore.Mvc;
using SysLog.Application.Dtos;
using SysLog.Application.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using OneForAll.Core.Extension;
using OneForAll.Core.OAuth;

namespace SysLog.Host.Controllers
{
    /// <summary>
    /// 企业活动
    /// </summary>
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ActivitysController : BaseController
    {
        private readonly IActivityService _service;

        public ActivitysController(IActivityService service)
        {
            _service = service;
        }

        /// <summary>
		/// 查询企业登录记录列表
		/// </summary>
        /// <param name="top">数据量</param>
		///  <returns>列表</returns>
		[HttpGet]
        [Route("Login/Logs/{top}")]
        public async Task<IEnumerable<SysLoginLogDto>> GetListLoginAsync(int top = 10)
        {
            if (top > 100) top = 100;
            // 未登录（管理界面免登录访问）时返回空列表
            if (LoginUser.Id.IsNullOrEmpty())
                return new List<SysLoginLogDto>();
            return await _service.GetListLoginAsync(top);
        }
    }
}
