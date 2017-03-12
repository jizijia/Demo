using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Core.Common;

namespace Demo.Core.Domain.Account
{
    /// <summary>
    /// 用户操作日志实体
    /// </summary>
    public class UserActivityLog : BaseEntity
    {
        /// <summary>
        /// 账户标识
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }        
        
        /// <summary>
        /// 用户活动类型
        /// 如下:
        ///     登录成功(LOGIN_SUCCEED)
        ///     登录失败(LOGIN_FAILED)
        ///     安全退出(LOGOUT)
        ///     请求(REQUEST)
        ///     其他(OTHER)
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 访问路径
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// 来源地址/或上次访问地址
        /// </summary>
        public string ReferrerUrl { get; set; }
    }
}
