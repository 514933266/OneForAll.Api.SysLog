using System;
using SysLog.Domain;
using SysLog.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SysLog.Host
{
    public partial class OneForAll_SysLogContext : DbContext
    {
        public OneForAll_SysLogContext(DbContextOptions<OneForAll_SysLogContext> options)
            : base(options)
        {
            
        }

		public virtual SysApiLog SysApiLog { get; set; }
        public virtual SysExceptionLog SysExceptionLog { get; set; }
        public virtual SysLoginLog SysLoginLog { get; set; }
        public virtual SysOperationLog SysOperationLog { get; set; }
        public virtual SysGlobalExceptionLog SysGlobalExceptionLog { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 系统日志

            modelBuilder.Entity<SysApiLog>(entity =>
            {
                entity.ToTable("Sys_ApiLog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysExceptionLog>(entity =>
            {
                entity.ToTable("Sys_ExceptionLog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysLoginLog>(entity =>
            {
                entity.ToTable("Sys_LoginLog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysOperationLog>(entity =>
            {
                entity.ToTable("Sys_OperationLog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysGlobalExceptionLog>(entity =>
            {
                entity.ToTable("Sys_GlobalExceptionLog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion
        }
    }
}
