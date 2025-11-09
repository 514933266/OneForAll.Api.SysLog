using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Public.Models
{
    /// <summary>
    /// 登录用户声明信息 (Login User Claim)
    /// 用于在用户成功认证后，封装并传递用户的核心身份标识和相关信息。
    /// 此类通常用于构建 JWT Token 的声明 (Claims) 或存储在会话中，避免频繁查询数据库。
    /// </summary>
    public class LoginUserClaim
    {
        /// <summary>
        /// 获取或设置用户的唯一标识符 (ID)。
        /// 通常作为用户在系统中的主键，用于关联用户相关的所有数据。
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置用户所属租户的唯一标识符 (Tenant ID)。
        /// 在多租户系统中，用于区分不同租户下的用户，实现数据隔离。
        /// </summary>
        public Guid SysTenantId { get; set; }

        /// <summary>
        /// 获取或设置用户的显示名称 (Name)。
        /// 通常是用户的真实姓名或昵称，用于在界面中展示给其他用户。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置用户的登录账号 (Username)。
        /// 用户用于登录系统的账户名，可能为邮箱、手机号或自定义用户名。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示该用户是否为租户的默认用户（如超级管理员或初始创建者）。
        /// 可用于控制特定的系统级权限或功能。
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 获取或设置与该用户关联的微信小程序的 AppID。
        /// 用于标识用户是通过哪个微信小程序进行授权登录的。
        /// </summary>
        public string WxAppId { get; set; }

        /// <summary>
        /// 获取或设置该用户在某个微信小程序内的唯一标识 (OpenID)。
        /// OpenID 是用户在单个微信小程序中的唯一身份 ID，不同小程序的 OpenID 不同。
        /// </summary>
        public string WxOpenId { get; set; }

        /// <summary>
        /// 获取或设置该用户在微信开放平台下的唯一标识 (UnionID)。
        /// 当用户在多个绑定到同一开放平台的小程序或公众号中登录时，UnionID 是相同的，可用于用户身份的跨应用统一。
        /// </summary>
        public string WxUnionId { get; set; }
    }
}