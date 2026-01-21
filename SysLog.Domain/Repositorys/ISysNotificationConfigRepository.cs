using OneForAll.EFCore;
using SysLog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain.Repositorys
{
    /// <summary>
    /// 系统通知
    /// </summary>
    public interface ISysNotificationConfigRepository : IEFCoreRepository<SysNotificationConfig>
    {
    }
}
