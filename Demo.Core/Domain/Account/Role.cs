using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Demo.Core;
using Demo.Core.Common;

namespace Demo.Core.Domain.Account
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role : BaseEntity
    {
        public Role()
        {
            FuntionResources = new List<Permission>();
        }

        /// <summary>
        /// 编码
        /// </summary> 
        public string Code { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary> 
        public string Description { get; set; }

        /// <summary>
        /// 该角色下的所有用户
        /// </summary>
        public ICollection<User> Users { get; set; }

        /// <summary>
        /// 是否为系统默认角色
        /// 系统角色不能修改、删除、启用或停用
        /// </summary>
        public string IsSystem { get; set; }

        /// <summary>
        /// 状态
        /// 状态：启用、停用
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 所拥有的控制器
        /// </summary>
        public virtual ICollection<Permission> FuntionResources { get; set; }
    }
}
