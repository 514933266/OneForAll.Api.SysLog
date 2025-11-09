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
    /// 登录日志
    /// </summary>
    public class SysLoginLogManager : SysBaseManager, ISysLoginLogManager
    {
        private readonly ISysLoginLogRepository _repository;

        public SysLoginLogManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysLoginLogRepository repository) : base(mapper, httpContextAccessor)
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
        public async Task<PageList<SysLoginLog>> GetPgaeAsync(
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
        public async Task<BaseErrType> AddAsync(SysLoginLogForm entity)
        {
            var data = _mapper.Map<SysLoginLogForm, SysLoginLog>(entity);
            if (data.CreateTime == DateTime.MinValue || data.CreateTime == DateTime.MaxValue)
                data.CreateTime = DateTime.UtcNow;
            return await ResultAsync(() => _repository.AddAsync(data));
        }
    }
}
