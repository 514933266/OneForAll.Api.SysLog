using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OneForAll.Core;
using SysLog.Domain.Entities;
using SysLog.Domain.Interfaces;
using SysLog.Domain.Models;
using SysLog.Domain.Repositorys;
using SysLog.HttpService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain
{
    /// <summary>
    /// 异常日志
    /// </summary>
    public class SysExceptionLogManager : BaseManager, ISysExceptionLogManager
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private readonly ISysExceptionLogRepository _repository;
        private readonly ISysFilterLogConfigRepository _filterRepository;

        private readonly ISysUmsMessageHttpService _umsHttpService;


        public SysExceptionLogManager(
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ISysExceptionLogRepository repository,
            ISysUmsMessageHttpService umsHttpService,
            ISysFilterLogConfigRepository filterRepositorys) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _configuration = configuration;
            _repository = repository;
            _filterRepository = filterRepositorys;
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

            var filterKey = $"{form.ModuleCode}|{form.Controller}|{form.Action}|All".ToLowerInvariant();
            var disallowedKeys = await _filterRepository.GetListCacheAsync();

            if (disallowedKeys.Contains(filterKey))
                return BaseErrType.NotAllow;

            return await ResultAsync(() => _repository.AddAsync(data));
        }
    }
}
