using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Base
{
    /// <summary>
    /// 用户登录票据
    /// </summary>
    public class UserTicket
    {
        public const string CODE = "1";

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

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }
}
