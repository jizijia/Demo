using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Core.Data;
using Demo.Core.Caching;
using Demo.Core;
using Demo.Core.Domain;
using Demo.Core.Common;
using Demo.Core.Domain.Account;

namespace Demo.Service
{
    /// <summary>
    /// 用户信息管理服务
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private IRepository<Permission> _permissionRepository;
        public PermissionService(IRepository<Permission> PermissionRepository)
        {
            _permissionRepository = PermissionRepository;
        }

        public List<Permission> AllPermission
        {
            get
            {
                if (this.AllPermission == null)
                {
                    AllPermission = GetAllPermissions();
                }
                return AllPermission;
            }
            set { this.AllPermission = value; }
        }

        public void Insert(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");
            _permissionRepository.Insert(permission);
        }

        /// <summary>
        /// 支持模糊检索
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<Permission> GetAllPermissions(string name, string code, string moduleName, string pageName, string elementName, string comment)
        {
            var query = _permissionRepository.Table;
            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.Contains(name));
            if (!string.IsNullOrEmpty(code))
                query = query.Where(c => c.Code.Contains(code));
            if (!string.IsNullOrEmpty(moduleName))
                query = query.Where(c => c.ModuleName.Contains(moduleName));
            if (!string.IsNullOrEmpty(pageName))
                query = query.Where(c => c.PageName.Contains(pageName));
            if (!string.IsNullOrEmpty(elementName))
                query = query.Where(c => c.ElementName.Contains(elementName));
            if (!string.IsNullOrEmpty(comment))
                query = query.Where(c => c.Comments.Contains(comment));
            List<Permission> result = query.ToList();

            List<Permission> list = new List<Permission>();
            List<Permission> allPermission = GetAllPermissions();
            foreach (var item in result)
            {
                foreach (var childItem in allPermission)
                {
                    if (childItem.Code.LeftLike(item.Code))
                    {
                        list.Add(childItem);
                    }
                }
            }
            return list.OrderBy(c => c.Code).ToList();
        }

        /// <summary>
        /// 通过唯一编号ID进行检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Permission GetByID(int id)
        {
            return _permissionRepository.GetByID(id);
        }

        public Permission GetPermissionByCode(string code)
        {
            return _permissionRepository.GetByCondition(c => c.Code.Contains(code));
        }

        public Permission GetPermission(string moduleName, string pageName, string elementName)
        {
            var query = _permissionRepository.Table;
            if (string.IsNullOrEmpty(moduleName))
            {
                query = query.Where(c => c.ModuleName == moduleName);
            }
            if (string.IsNullOrEmpty(pageName))
            {
                query = query.Where(c => c.PageName == pageName);
            }

            if (string.IsNullOrEmpty(elementName))
            {
                query = query.Where(c => c.ElementName == elementName);
            }

            var user = query.FirstOrDefault();
            return user;
        }

        public List<Permission> GetPublicPermissionList()
        {
            var query = _permissionRepository.Table;
            query = query.Where(c => c.IsPublic == "Y");
            return query.ToList();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        public void DeletePermission(Permission Permission)
        {
            if (Permission == null)
                throw new ArgumentNullException("Permission");
            _permissionRepository.Delete(Permission);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        public void UpdatePermission(Permission Permission)
        {
            if (Permission == null)
                throw new ArgumentNullException("Permission");
            _permissionRepository.Update(Permission);
        }

        public List<Permission> GetAllPermissions()
        {
            
            return _permissionRepository.Table.ToList();

            var query = from c in _permissionRepository.Table
                        orderby c.Code
                        select c;
            return query.ToList();
        }
       
        private void ReversePermission(Permission childAceessController, ref List<Permission> result)
        {
            if (childAceessController.Code.Length <= 2)
            {
                return;
            }
            string parentCode = childAceessController.Code.Substring(0, childAceessController.Code.Length - 2);
            Permission controller = GetPermissionByCode(parentCode);
            if (controller != null)
                result.Add(controller);
            else
                return;
            ReversePermission(controller, ref result);
        }
    }
}
