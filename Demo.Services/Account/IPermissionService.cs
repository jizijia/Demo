using System.Collections.Generic;
using Demo.Core.Domain;
using Demo.Core.Domain.Account;

namespace Demo.Service
{
    public interface IPermissionService
    {
        List<Permission> AllPermission { get; set; }

        void DeletePermission(Permission Permission);
        List<Permission> GetAllPermissions();
        List<Permission> GetAllPermissions(string name, string code, string moduleName, string pageName, string elementName, string comment);
        Permission GetByID(int id);
        Permission GetPermission(string moduleName, string pageName, string elementName);
        Permission GetPermissionByCode(string code);
        List<Permission> GetPublicPermissionList();
        void Insert(Permission permission);
        void UpdatePermission(Permission Permission);
    }
}