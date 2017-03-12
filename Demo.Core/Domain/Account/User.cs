using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Data;
using Demo.Core;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Demo.Core.Common;

namespace Demo.Core.Domain.Account
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : BaseEntity
    {
        public User()
        { 
            DateCreated = DateTime.Now;
            DateLastPasswordChaged = DateTime.Now;
            Permissions = new List<Permission>();
            Roles = new List<Role>();
        }
        /// <summary>
        /// 账户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// 确认码
        /// </summary>
        public string ValidCode { get; set; }

        /// <summary>
        /// 注册时使用的IP
        /// </summary>
        public string RegisterIP { get; set; }        

        /// <summary>
        /// 所属角色
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// 状态
        /// 启用（ENABLE）、停用（DISABLE）
        /// </summary> 
        public string Status { get; set; }

        /// <summary>
        /// 是否为系统用户
        /// 是（Y）/否（N）
        /// </summary> 
        public string IsSystem { get; set; }

        /// <summary>
        /// 是否在线
        /// 是（Y）/否（N）
        /// </summary> 
        public string IsOnline { get; set; }
        
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? DateRegister { get; set; }

        /// <summary>
        /// 最后一次登录的时间
        /// </summary> 
        public DateTime? DateLastLogin { get; set; }
        
        /// <summary>
        /// 最后一次状态变更时间
        /// </summary> 
        public DateTime? DateStatusChanged { get; set; }        

        /// <summary>
        /// 最后一次密码变更的时间
        /// </summary> 
        public DateTime? DateLastPasswordChaged { get; set; }

        /// <summary>
        /// 所有的控制器
        /// </summary> 
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
