using System;

namespace SysLog.Application.Dtos
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public class SysApiLogDto
    {
        /// <summary>
        /// 所属模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 控制器方法
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 请求域名
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求方法：GET、POST、PUT、DELETE等
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 请求类型，如果是文件类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 请求Body内容，如果是文件上传类不记录
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// 响应内容，如果是下载不记录
        /// </summary>
        public string ReponseBody { get; set; }

        /// <summary>
        /// 完整的浏览器信息
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 消耗时间
        /// </summary>
        public string TimeSpan { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
