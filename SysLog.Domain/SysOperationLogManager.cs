using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OneForAll.Core;
using SysLog.Domain.Models;
using SysLog.Domain.Interfaces;
using SysLog.Domain.Repositorys;
using SysLog.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace SysLog.Domain
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class SysOperationLogManager : BaseManager, ISysOperationLogManager
    {
        private readonly IMapper _mapper;
        private readonly ISysOperationLogRepository _repository;
        private readonly ISysFilterLogConfigRepository _filterRepository;
        public SysOperationLogManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysOperationLogRepository repository,
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
        public async Task<PageList<SysOperationLog>> GetPgaeAsync(
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
        public async Task<BaseErrType> AddAsync(SysOperationLogForm form)
        {
            var data = _mapper.Map<SysOperationLogForm, SysOperationLog>(form);
            if (data.CreateTime == DateTime.MinValue || data.CreateTime == DateTime.MaxValue)
                data.CreateTime = DateTime.UtcNow;

            var filterKey = $"{form.ModuleCode}|{form.Controller}|{form.Action}".ToLowerInvariant();
            var disallowedKeys = await _filterRepository.GetListCacheAsync();

            if (disallowedKeys.Contains(filterKey))
                return BaseErrType.NotAllow;

            return await ResultAsync(() => _repository.AddAsync(data));
        }
    }
}
