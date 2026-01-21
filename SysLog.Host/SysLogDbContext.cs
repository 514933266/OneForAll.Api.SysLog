using SysLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        public virtual SysFilterLogConfig SysFilterLogConfig { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysApiLog>(entity =>
            {
                entity.ToTable("sys_api_log");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysExceptionLog>(entity =>
            {
                entity.ToTable("sys_exception_log");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysLoginLog>(entity =>
            {
                entity.ToTable("sys_login_log");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysOperationLog>(entity =>
            {
                entity.ToTable("sys_operation_log");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysGlobalExceptionLog>(entity =>
            {
                entity.ToTable("sys_global_exception_log");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysFilterLogConfig>(entity =>
            {
                entity.ToTable("sys_filter_log_config");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
