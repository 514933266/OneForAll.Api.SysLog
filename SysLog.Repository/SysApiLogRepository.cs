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
using System.Collections.Generic;
using SysLog.Domain.ValueObjects;

namespace SysLog.Repository
{
	/// <summary>
	/// Api日志
	/// </summary>
	public class SysApiLogRepository : Repository<SysApiLog>, ISysApiLogRepository
	{
		public SysApiLogRepository(DbContext context)
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
		/// <param name="controller">控制器</param>
		/// <param name="action">方法</param>
		/// <param name="key">关键字</param>
		///  <returns>分页</returns>
		public async Task<PageList<SysApiLog>> GetPgaeAsync(
			int pageIndex,
			int pageSize,
			DateTime? startTime,
			DateTime? endTime,
			string userName,
			string controller,
			string action,
			string key)
		{
			var predicate = PredicateBuilder.Create<SysApiLog>(w => true);

			if (!userName.IsNullOrEmpty())
				predicate = predicate.And(w => w.CreatorName == userName);
			if (!controller.IsNullOrEmpty())
				predicate = predicate.And(w => w.Controller == controller);
			if (!action.IsNullOrEmpty())
				predicate = predicate.And(w => w.Action == action);
			if (startTime != null)
				predicate = predicate.And(w => w.CreateTime >= startTime);
			if (endTime != null)
				predicate = predicate.And(w => w.CreateTime <= endTime);
			if (!key.IsNullOrEmpty())
				predicate = predicate.And(w => w.ModuleCode.Contains(key) || w.ModuleName.Contains(key));

			var total = await DbSet.AsNoTracking().CountAsync(predicate);

			var items = await DbSet
				.AsNoTracking()
				.Where(predicate)
				.OrderByDescending(w => w.CreateTime)
				.Skip(pageSize * (pageIndex - 1))
				.Take(pageSize)
				.ToListAsync();

			return new PageList<SysApiLog>(total, pageIndex, pageSize, items);
		}

		/// <summary>
		/// 查询用户日志列表
		/// </summary>
		/// <param name="tenantId">机构id</param>
		/// <param name="userId">用户id</param>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		///  <returns>分页</returns>
		public async Task<IEnumerable<UserLivenessApiLogVo>> GetListAsync(string tenantId, string userId, DateTime startTime, DateTime endTime)
		{
			return await DbSet
				.AsNoTracking()
				.Where(w => w.SysTenantId == tenantId && w.CreatorId == userId && w.CreateTime >= startTime && w.CreateTime <= endTime)
				.Select(s => new UserLivenessApiLogVo() { Method = s.Method, CreatorId = s.CreatorId, CreateTime = s.CreateTime })
				.ToListAsync();
		}

		/// <summary>
		/// 查询用户日志列表
		/// </summary>
		/// <param name="tenantId">机构id</param>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		///  <returns>分页</returns>
		public async Task<IEnumerable<UserLivenessApiLogVo>> GetListAsync(string tenantId, DateTime startTime, DateTime endTime)
		{
			return await DbSet
				.AsNoTracking()
				.Where(w => w.SysTenantId == tenantId && w.CreateTime >= startTime && w.CreateTime <= endTime)
				.Select(s => new UserLivenessApiLogVo() { Method = s.Method, CreatorId = s.CreatorId, CreateTime = s.CreateTime })
				.ToListAsync();
		}
	}
}
