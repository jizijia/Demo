using System;
using Demo.Core.Domain;
using System.Collections.Generic;
using Demo.Core.Domain.Account;

namespace Demo.Service
{
    public interface IRoleService
    {
        void Insert(Role role);
        void Delete(int id);
        Demo.Core.PagedList<Role> GetAllRoles(int pageIndex = 0, int pageSize = 2147483647);
        Role GetByID(int id);
        Role GetByName(string roleName);
        Role GetByCode(string roleCode);
        void Update(Role role);
        void SetStatus(int id, string status);
    }
}
