using SysLog.Domain.AggregateRoots;
using SysLog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain.Aggregates
{
    /// <summary>
    /// 用户活跃度
    /// </summary>
    public class UserLivenessAggr
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 每日活跃度
        /// </summary>
        public List<UserLivenessDateVo> Dates { get; set; } = new List<UserLivenessDateVo>();

        /// <summary>
        /// 初始化日期范围
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void InitDate(DateTime startTime, DateTime endTime)
        {
            var day = (endTime.Date - startTime.Date).Days;
            for (int i = 0; i < day; i++)
            {
                Dates.Add(new UserLivenessDateVo()
                {
                    Date = startTime.AddDays(i)
                });
            }
        }

        /// <summary>
        /// 根据日志计算活跃度分数
        /// </summary>
        /// <param name="logs">请求日志</param>
        /// <returns></returns>
        public void CalculateDateScope(IEnumerable<UserLivenessApiLogVo> logs)
        {
            foreach (var date in Dates)
            {
                var items = logs.Where(w => w.CreateTime.Date == date.Date).ToList();
                if (items.Any())
                {
                    var scope = 0;
                    foreach (var log in items)
                    {
                        switch (log.Method)
                        {
                            case "GET": scope += 2; break;
                            case "POST": scope += 6; break;
                            case "PUT": scope += 5; break;
                            case "PATCH": scope += 3; break;
                            case "DELETE": scope += 4; break;
                            default: scope += 1; break;
                        }
                    }
                    date.Value = scope;
                }
            }
        }
    }
}
