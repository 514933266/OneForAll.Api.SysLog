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

namespace SysLog.Domain
{
    /// <summary>
    /// 全局异常
    /// </summary>
    public class SysGlobalExceptionLogManager : SysBaseManager, ISysGlobalExceptionLogManager
    {
        private readonly ISysGlobalExceptionLogRepository _repository;

        public SysGlobalExceptionLogManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysGlobalExceptionLogRepository repository) : base(mapper, httpContextAccessor)
        {
            _repository = repository;
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
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(SysGlobalExceptionLogForm entity)
        {
            var data = _mapper.Map<SysGlobalExceptionLogForm, SysGlobalExceptionLog>(entity);
            if (data.CreateTime == DateTime.MinValue || data.CreateTime == DateTime.MaxValue)
                data.CreateTime = DateTime.UtcNow;
            return await ResultAsync(() => _repository.AddAsync(data));
        }
    }
}
