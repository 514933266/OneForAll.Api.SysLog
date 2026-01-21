using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using SysLog.Domain.Entities;
using SysLog.Domain.Interfaces;
using SysLog.Domain.Models;
using SysLog.Domain.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysLog.Domain
{
    /// <summary>
    /// 全局异常
    /// </summary>
    public class SysGlobalExceptionLogManager : BaseManager, ISysGlobalExceptionLogManager
    {
        private readonly IMapper _mapper;
        private readonly ISysGlobalExceptionLogRepository _repository;
        private readonly ISysFilterLogConfigRepository _filterRepository;
        public SysGlobalExceptionLogManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysGlobalExceptionLogRepository repository,
            ISysFilterLogConfigRepository filterRepository) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repository = repository;
            _filterRepository = filterRepository;
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
        public async Task<PageList<SysGlobalExceptionLog>> GetPgaeAsync(
                int pageIndex,
                int pageSize,
                DateTime? startTime,
                DateTime? endTime,
                string userName,
                string key)
        {
            return await _repository.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, userName, key);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(SysGlobalExceptionLogForm form)
        {
            var data = _mapper.Map<SysGlobalExceptionLogForm, SysGlobalExceptionLog>(form);
            if (data.CreateTime == DateTime.MinValue || data.CreateTime == DateTime.MaxValue)
                data.CreateTime = DateTime.UtcNow;

            var filterKey = $"{form.ModuleCode}|无|{form.Name}|All".ToLowerInvariant();
            var disallowedKeys = await _filterRepository.GetListCacheAsync();

            if (disallowedKeys.Contains(filterKey))
                return BaseErrType.NotAllow;

            return await ResultAsync(() => _repository.AddAsync(data));
        }
    }
}
