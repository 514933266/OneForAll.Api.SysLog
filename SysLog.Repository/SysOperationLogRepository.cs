using System.Linq;
using System.Threading.Tasks;
using OneForAll.Core;
using OneForAll.EFCore;
using OneForAll.Core.ORM;
using OneForAll.Core.Extension;
using Microsoft.EntityFrameworkCore;
using SysLog.Domain.Repositorys;
using SysLog.Domain.AggregateRoots;

namespace SysLog.Repository
{
	/// <summary>
	/// 系统操作日志
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
		/// <param name="key">关键字：系统名称 模块名称</param>
		///  <returns>分页</returns>
		public async Task<PageList<SysOperationLog>> GetPageAsync(int pageIndex, int pageSize, string key)
		{
			var predicate = PredicateBuilder.Create<SysOperationLog>(w => true);

			if (!key.IsNullOrEmpty())
			{
				predicate = predicate.And(w => w.SystemName.Contains(key) || w.ModuleName.Contains(key));
			}

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

			return new PageList<SysOperationLog>(total, pageIndex, pageSize, items);
		}

	}
}
