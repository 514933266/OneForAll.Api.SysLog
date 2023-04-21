using System;
using SysLog.Domain;
using SysLog.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SysLog.Host
{
    public partial class OneForAllContext : DbContext
    {
        public OneForAllContext(DbContextOptions<OneForAllContext> options)
            : base(options)
        {
            
        }

		public virtual SysApiLog SysApiLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 系统日志

            modelBuilder.Entity<SysApiLog>(entity =>
            {
                entity.ToTable("Sys_ApiLog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion
        }
    }
}
