using System;
using Demo.Core.Domain.Account;
using System.Collections.Generic;
using Demo.Core.Base;

namespace Demo.Service
{
    public interface IUserService
    {
         bool IsExisting(string userName);
        BaseResult ChangePassword(User user, string oldPassword, string newPassword);
        void DeleteUser(User user);
        User GetUserByID(int id);
        User GetUserByUserName(string userName);
        User Insert(User user);
        string ResetPassword(User user);
        BaseResult UpdateUser(User user);
        BaseResult<User> Login(string userName, string password, string code);
        bool Validate(string userName, string password, out string message);
    }
}
