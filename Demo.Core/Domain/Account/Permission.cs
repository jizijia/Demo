using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Data;
using Demo.Core;
using Demo.Core.Common;

namespace Demo.Core.Domain.Account
{
    /// <summary>
    /// 权限控制器
    /// </summary>
    public class Permission : BaseEntity //,ITreeBaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 界面名称
        /// </summary>
        public string PageName { get; set; }

        /// <summary>
        /// 元素名称
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 是否公共(Y/N)
        /// </summary>
        public string IsPublic { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 权限类型
        /// 权限类型分为模块、界面、界面元素
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 状态
        /// 状态：新增（NEW）、启用（ENABLE）、停用（DISABLE）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 拥有本权限的所有角色
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }
    }
}
