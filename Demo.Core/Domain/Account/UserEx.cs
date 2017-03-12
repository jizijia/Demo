using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Core.Domain.Account
{
    public static class UserEx
    {
        /// <summary>
        /// 获取一个值,该值指示用户是否在某个客户角色
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="customerRoleSystemName">用户角色</param>
        /// <param name="onlyActiveCustomerRoles">该值指示我们是否应该只在活跃客户角色</param>
        /// <returns>Result</returns>
        public static bool IsInRole(this User user,
            string role, bool onlyActiveCustomerRoles = true)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(role))
                throw new ArgumentNullException("customerRoleSystemName");
            if (user.Roles == null || user.Roles.Count < 1)
            {
                return false;
            }
            foreach (var item in user.Roles)
            {
                if (item.Name == role)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断用户是否为管理员(拥有最高角色)
        /// </summary>
        /// <param name="customer">用户</param> 
        /// <returns>Result</returns>
        public static bool IsMaster(this User user)
        {
            if (user.UserName == "admin")  //只运行admin拥有最高的权限
                return true;
            return false;
        }

        /// <summary>
        ///判断用户是否已经注册
        /// </summary>
        /// <param name="customer">用户</param> 
        /// <returns>Result</returns>
        public static bool IsRegistered(this User user)
        {
            return (user.Roles != null && user.Roles.Count < 1);
        }
        
        /// <summary>
        /// 是否为来宾用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsGuest(this User user)
        {
            return (user.Roles == null || user.Roles.Count < 1);
        } 
    }
}
