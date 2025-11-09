using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Public.Models
{
    /// <summary>
    /// 用户声明（Claim）类型常量。
    /// </summary>
    public class UserClaimType
    {
        /// <summary>
        /// 租户ID。
        /// </summary>
        public const string TenantId = "TenantId";

        /// <summary>
        /// 用户名。
        /// </summary>
        public const string UserName = "UserName";

        /// <summary>
        /// 用户昵称。
        /// </summary>
        public const string UserNickName = "UserNickName";

        /// <summary>
        /// 用户ID。
        /// </summary>
        public const string UserId = "UserId";

        /// <summary>
        /// 人员ID。
        /// </summary>
        public const string PersonId = "PersonId";

        /// <summary>
        /// 是否为默认。
        /// </summary>
        public const string IsDefault = "IsDefault";

        /// <summary>
        /// 微信小程序AppId。
        /// </summary>
        public const string WxAppId = "WxAppId";

        /// <summary>
        /// 微信OpenId。
        /// </summary>
        public const string WxOpenId = "WxOpenId";

        /// <summary>
        /// 微信UnionId。
        /// </summary>
        public const string WxUnionId = "WxUnionId";
    }
}
