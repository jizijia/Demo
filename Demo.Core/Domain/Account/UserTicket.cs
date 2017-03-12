using System;

namespace Demo.Core.Domain.Account
{
    /// <summary>
    /// 用户登录票据
    /// </summary>
    public class UserTicket
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 是否为后台管理员
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 登录IP
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 是否验证IP
        /// </summary>
        public bool IpRequired { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ExpiryDate { get; set; }
    }
}
