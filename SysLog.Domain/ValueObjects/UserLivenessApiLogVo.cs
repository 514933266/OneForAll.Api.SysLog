using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain.ValueObjects
{
    /// <summary>
    /// 活跃度日志
    /// </summary>
    public class UserLivenessApiLogVo
    {
        /// <summary>
        /// 请求方法：GET、POST、PUT、DELETE等
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
