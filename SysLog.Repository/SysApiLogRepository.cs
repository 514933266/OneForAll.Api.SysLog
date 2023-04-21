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
		/// <param name="key">关键字</param>
		///  <returns>分页</returns>
		public async Task<PageList<SysApiLog>> GetPgaeAsync(
			int pageIndex,
			int pageSize,
			DateTime? startTime,
			DateTime? endTime,
			string userName,
			string key)
		{
			var predicate = PredicateBuilder.Create<SysApiLog>(w => true);

			if (!userName.IsNullOrEmpty())
				predicate = predicate.And(w => w.CreatorName.Contains(key));

			if (startTime != null)
				predicate = predicate.And(w => w.CreateTime >= startTime);

			if (endTime != null)
				predicate = predicate.And(w => w.CreateTime <= endTime);

			if (!key.IsNullOrEmpty())
				predicate = predicate.And(w => w.MoudleCode.Contains(key) || w.Controller.Contains(key) || w.Action.Contains(key));

			var total = await DbSet
			.AsNoTracking()
			.CountAsync(predicate);

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
		/// 添加
		/// </summary>
		/// <param name="entity">实体</param>
		///  <returns>结果</returns>
		public int Add(SysApiLog entity)
		{
			DbSet.Add(entity);
			return Context.SaveChanges();
		}
	}
}
