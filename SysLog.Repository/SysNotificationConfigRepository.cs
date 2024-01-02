using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using SysLog.Domain.AggregateRoots;
using SysLog.Domain.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Repository
{
    /// <summary>
    /// 系统通知设置
    /// </summary>
    public class SysNotificationConfigRepository : Repository<SysNotificationConfig>, ISysNotificationConfigRepository
    {
        public SysNotificationConfigRepository(DbContext context)
                  : base(context)
        {
        }
    }
}
