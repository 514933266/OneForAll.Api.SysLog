using System;
using System.Collections.Generic;
using System.Text;

namespace SysLog.Host
{
    public interface ITenantProvider
    {
        Guid GetTenantId();
    }
}
