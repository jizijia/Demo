using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Core.Data;
using Demo.Core.Caching;
using Demo.Core;
using Demo.Core.Domain;
using Demo.Core.Domain.Account;

namespace Demo.Service
{
    public class RoleService : Demo.Service.IRoleService
    {
        private readonly IRepository<Role> _roleRepository;

        public RoleService(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user"></param>
        public void Insert(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            _roleRepository.Insert(role);
        }

        /// <summary>
        /// 通过ID获取角色实例
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public Role GetByID(int id)
        {
            return _roleRepository.GetByID(id);
        }

        /// <summary>
        /// 通过角色名获取实例
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Role GetByName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");
            var query = from c in _roleRepository.Table
                        where c.Name == roleName
                        orderby c.ID
                        select c;
            return query.FirstOrDefault();
        }
        /// <summary>
        /// 通过角色名获取实例
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Role GetByCode(string roleCode)
        {
            if (string.IsNullOrEmpty(roleCode))
                throw new ArgumentNullException("roleCode");
            var query = from c in _roleRepository.Table
                        where c.Code == roleCode
                        orderby c.ID
                        select c;
            return query.FirstOrDefault();
        }

        public void Update(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            _roleRepository.Update(role);
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public PagedList<Role> GetAllRoles(int pageIndex = 0, int pageSize = 2147483647)
        { 
            var query = _roleRepository.Table.OrderBy(c => c.Code);
            var roles = new PagedList<Role>(query, pageIndex, pageSize);
            return roles;
        }

        public void SetStatus(int id, string status)
        {
            Role role = _roleRepository.GetByID(id);

            _roleRepository.UpdateSetByCondition(c => c.ID == id, (c) =>
            {
                c.Status = status;
                return c;
            });
        }

        public void Delete(int id)
        {
            Role role = _roleRepository.GetByID(id);
            _roleRepository.Delete(role);
        }

    }
}
