using System;
using System.Threading.Tasks;
using OneForAll.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SysLog.Domain.Models;
using SysLog.Public.Models;
using SysLog.Application.Dtos;
using SysLog.Application.Interfaces;

namespace SysLog.Host.Controllers
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.RULER)]
    public class SysExceptionLogsController : BaseController
    {
        private readonly ISysOperationLogService _service;

        public SysExceptionLogsController(ISysOperationLogService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        public async Task<PageList<SysOperationLogDto>> GetListAsync(int pageIndex, int pageSize, [FromQuery] string key)
        {
            return await _service.GetPgaeAsync(pageIndex, pageSize, key);
        }

        /// <summary>
        /// 添加
        /// </summary>
        [HttpPost]
        public async Task<BaseMessage> AddAsync([FromBody] SysOperationLogForm entity)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.AddAsync(entity);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("添加成功");
                default: return msg.Fail("添加失败");
            }
        }
    }
}
