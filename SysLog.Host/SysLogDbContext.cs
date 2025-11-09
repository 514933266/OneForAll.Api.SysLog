using System;
using SysLog.Domain;
using SysLog.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SysLog.Host
{
    public partial class SysLogDbContext : DbContext
    {
        public SysLogDbContext(DbContextOptions<SysLogDbContext> options)
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
                entity.ToTable("sys_apilog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysExceptionLog>(entity =>
            {
                entity.ToTable("sys_exceptionlog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysLoginLog>(entity =>
            {
                entity.ToTable("sys_loginlog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysOperationLog>(entity =>
            {
                entity.ToTable("sys_operationlog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysGlobalExceptionLog>(entity =>
            {
                entity.ToTable("sys_globalexceptionlog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion
        }
    }
}
