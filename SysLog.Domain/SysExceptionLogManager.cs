using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using SysLog.Domain.Models;
using SysLog.Domain.Interfaces;
using SysLog.Domain.Repositorys;
using SysLog.Domain.AggregateRoots;
using Microsoft.AspNetCore.Http;
using System.Text;
using SysLog.HttpService.Models;
using Microsoft.Extensions.Configuration;
using SysLog.HttpService.Interfaces;

namespace SysLog.Domain
{
    /// <summary>
    /// 异常日志
    /// </summary>
    public class SysExceptionLogManager : SysBaseManager, ISysExceptionLogManager
    {
        private readonly IConfiguration _configuration;
        private readonly ISysExceptionLogRepository _repository;
        private readonly ISysUmsMessageHttpService _umsHttpService;

        public SysExceptionLogManager(
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ISysExceptionLogRepository repository,
            ISysUmsMessageHttpService umsHttpService) : base(mapper, httpContextAccessor)
        {
            _configuration = configuration;
            _repository = repository;
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
        public async Task<PageList<SysExceptionLog>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string userName,
            string controller,
            string action,
            string key)
        {
            return await _repository.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, userName, controller, action, key);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(SysExceptionLogForm form)
        {
            var data = _mapper.Map<SysExceptionLogForm, SysExceptionLog>(form);
            if (data.CreateTime == DateTime.MinValue || data.CreateTime == DateTime.MaxValue)
                data.CreateTime = DateTime.UtcNow;
            return await ResultAsync(() => _repository.AddAsync(data));
        }
    }
}
