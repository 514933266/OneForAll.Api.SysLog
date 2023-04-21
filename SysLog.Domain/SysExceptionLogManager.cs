using System;
using System.Threading.Tasks;
using AutoMapper;
using OneForAll.Core;
using OneForAll.Core.DDD;
using SysLog.Domain.Models;
using SysLog.Domain.Interfaces;
using SysLog.Domain.Repositorys;
using SysLog.Domain.AggregateRoots;

namespace SysLog.Domain
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class SysExceptionLogManager : BaseManager, ISysOperationLogManager
    {
        private readonly IMapper _mapper;
        private readonly ISysOperationLogRepository _repository;

        public SysExceptionLogManager(
            IMapper mapper,
            ISysOperationLogRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字：系统名称 模块名称</param>
        public async Task<PageList<SysOperationLog>> GetPgaeAsync(int pageIndex, int pageSize, string key)
        {
            return await _repository.GetPageAsync(pageIndex, pageSize,key);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(SysOperationLogForm entity)
        {
            var data = _mapper.Map<SysOperationLogForm, SysOperationLog>(entity);

            data.Id = Guid.NewGuid();
            data.CreateTime = DateTime.Now;

            return await ResultAsync(() => _repository.AddAsync(data));
        }
    }
}
