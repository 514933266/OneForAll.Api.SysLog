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
	/// 登录日志
	/// </summary>
	public class SysLoginLogRepository : Repository<SysLoginLog>, ISysLoginLogRepository
	{
		public SysLoginLogRepository(DbContext context)
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
		public async Task<PageList<SysLoginLog>> GetPgaeAsync(
			int pageIndex,
			int pageSize,
			DateTime? startTime,
			DateTime? endTime,
			string userName,
			string key)
		{
			var predicate = PredicateBuilder.Create<SysLoginLog>(w => true);

			if (!userName.IsNullOrEmpty())
				predicate = predicate.And(w => w.CreatorName.Contains(key));

			if (startTime != null)
				predicate = predicate.And(w => w.CreateTime >= startTime);

			if (endTime != null)
				predicate = predicate.And(w => w.CreateTime <= endTime);

			var total = await DbSet.AsNoTracking().CountAsync(predicate);

			var items = await DbSet
				.AsNoTracking()
				.Where(predicate)
				.OrderByDescending(w => w.CreateTime)
				.Skip(pageSize * (pageIndex - 1))
				.Take(pageSize)
				.ToListAsync();

			return new PageList<SysLoginLog>(total, pageIndex, pageSize, items);
		}

		/// <summary>
		/// 查询机构登录记录列表
		/// </summary>
		/// <param name="tenantId">租户Id</param>
		/// <param name="top">数据量</param>
		///  <returns>登录列表</returns>
		public async Task<IEnumerable<SysLoginLog>> GetListTodayAsync(string tenantId, int top)
		{
			var predicate = PredicateBuilder.Create<SysLoginLog>(w => w.SysTenantId == tenantId && w.CreateTime >= DateTime.UtcNow.Date);

			return await DbSet
				.AsNoTracking()
				.Where(predicate)
				.OrderByDescending(w => w.CreateTime)
				.Take(top)
				.ToListAsync();
		}
	}
}
