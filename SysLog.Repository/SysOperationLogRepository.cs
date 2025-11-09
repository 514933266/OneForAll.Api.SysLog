using System.Linq;
using System.Threading.Tasks;
using OneForAll.Core;
using OneForAll.EFCore;
using OneForAll.Core.ORM;
using OneForAll.Core.Extension;
using Microsoft.EntityFrameworkCore;
using SysLog.Domain.Repositorys;
using SysLog.Domain.AggregateRoots;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using SysLog.Domain.Aggregates;
using SysLog.Domain.ValueObjects;

namespace SysLog.Repository
{
	/// <summary>
	/// 操作日志
	/// </summary>
	public class SysOperationLogRepository : Repository<SysOperationLog>, ISysOperationLogRepository
	{
		public SysOperationLogRepository(DbContext context)
				  : base(context)
		{
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
			var predicate = PredicateBuilder.Create<SysOperationLog>(w => true);

			if (!userName.IsNullOrEmpty())
				predicate = predicate.And(w => w.CreatorName.Contains(key));

			if (startTime != null)
				predicate = predicate.And(w => w.CreateTime >= startTime);

			if (endTime != null)
				predicate = predicate.And(w => w.CreateTime <= endTime);

			if (!key.IsNullOrEmpty())
				predicate = predicate.And(w => w.ModuleCode.Contains(key) || w.Controller.Contains(key) || w.Action.Contains(key));

			var total = await DbSet.AsNoTracking().CountAsync(predicate);

			var items = await DbSet
				.AsNoTracking()
				.Where(predicate)
				.OrderByDescending(w => w.CreateTime)
				.Skip(pageSize * (pageIndex - 1))
				.Take(pageSize)
				.ToListAsync();

			return new PageList<SysOperationLog>(total, pageIndex, pageSize, items);
		}
    }
}
