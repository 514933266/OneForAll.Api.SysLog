using SysLog.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Application.Interfaces
{
    /// <summary>
    /// 企业活动
    /// </summary>
    public interface IActivityService
    {
        /// <summary>
		/// 查询登录列表
		/// </summary>
		///  <returns>列表</returns>
        Task<IEnumerable<SysLoginLogDto>> GetListLoginAsync(int top);
    }
}
