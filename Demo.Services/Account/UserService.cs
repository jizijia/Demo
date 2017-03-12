using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Core.Data;
using Demo.Core.Caching;
using Demo.Core;
using Demo.Core.Domain;
using Demo.Service;
using Demo.Core.Common;
using Demo.Core.Domain.Account;
using Demo.Core.Base;
using Demo.Services.Account;

namespace Demo.Service
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserService : IUserService
    {

        private readonly IRepository<User> _userRepository;
        private readonly IUserActivityLogService _userlogRepository;
        public UserService(IRepository<User> userRespository,IUserActivityLogService userlogRepository)
        {
            _userRepository = userRespository;
            _userlogRepository = userlogRepository;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户实体类</param>
        public User Insert(User user)
        {
            if (user == null)
                throw new BaseResultException { Code=BaseCode.数据验证失败,ResultMessage="数据异常"};

            if (IsExisting(user.UserName))
                throw new BaseResultException { Code = BaseCode.数据验证失败, ResultMessage = "该用户已存在" };

            return _userRepository.Insert(user);
        }

        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName">账户名称</param>
        /// <returns>是否操作成功</returns>
        public bool IsExisting(string userName)
        {
            return _userRepository.Exists(c => c.UserName == userName);
        }

        /// <summary>
        /// 通过唯一编号ID进行检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserByID(int id)
        {
            return _userRepository.GetByID(id);
        }

        /// <summary>
        /// 通过用户名进行检索
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return null;
            var query = from c in _userRepository.Table
                        where c.UserName == userName
                        orderby c.ID
                        select c;
            var user = query.FirstOrDefault();
            return user;

        }

        /// <summary>
        /// 用户验证,并更新最后活动日期
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password">有加密过的密码</param>
        /// <returns></returns>
        public bool Validate(string userName, string password, out string message)
        {
            message = string.Empty;
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                return false;
            User user = GetUserByUserName(userName);
            if (user == null)
            {
                message = "用户名不存在.";
                return false;
            }
            if (user.Status == "DISABLE")
            {
                message = "您的账户已经被锁定,请联系管理员进行解锁.";
                return false;
            }
            if (user.Password != GeneratePassword(password))
            {
                message = "密码错误.";
                return false;
            }
            user.DateLastLogin = DateTime.Now;
            return true;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(User user)
        {
            if (user == null)
                throw new BaseResultException { Code=BaseCode.数据验证失败,ResultMessage="数据异常"};

            _userRepository.Delete(user);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        public BaseResult UpdateUser(User user)
        {
            if (user == null)
                throw new BaseResultException { Code = BaseCode.数据验证失败, ResultMessage = "数据异常" };

            return new BaseResult {
                Data = _userRepository.Update(user)
            };
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns>返回新的随机密码</returns>
        public string ResetPassword(User user)
        {
            string radonPassword = RandomExtentions.GetRandonString(8);
            user.Password = GeneratePassword(radonPassword);
            user.DateLastPasswordChaged = DateTime.Now;
            _userRepository.Update(user);
            return radonPassword;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="newPassword"></param>
        public BaseResult ChangePassword(User user, string oldPassword, string newPassword)
        {
            if (user == null)
                throw new BaseResultException { Code=BaseCode.数据验证失败,ResultMessage="数据异常"};

            if (oldPassword.IsNone())
                throw new BaseResultException { Code=BaseCode.数据验证失败,ResultMessage="旧密码不能为空"};

            if (newPassword.IsNone())
                throw new BaseResultException { Code=BaseCode.数据验证失败,ResultMessage="新密码不能为空"};

            if (user.Password != GeneratePassword(oldPassword))
                throw new BaseResultException { Code = BaseCode.数据验证失败, ResultMessage = "旧密码错误" };

            user.Password = GeneratePassword(newPassword);
            return new BaseResult {
                Data= _userRepository.Update(user)
            };
        }
        
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public BaseResult<User> Login(string userName,string password,string code)
        {
            try
            {
                if (userName.IsNone())
                    throw new BaseResultException { Code = BaseCode.数据验证失败, ResultMessage = "用户名不能为空" };

                if (password.IsNone())
                    throw new BaseResultException { Code = BaseCode.数据验证失败, ResultMessage = "密码不能为空" };

                var user = _userRepository.GetByCondition(x => x.UserName == userName);
                if (user == null)
                    throw new BaseResultException { Code = BaseCode.查无数据, ResultMessage = "用户不存在" };

                if (user.Status == "DISABLE")
                    throw new BaseResultException { Code = BaseCode.查无数据, ResultMessage = "该账户已停用，请联系管理员进行解锁" };

                var loginFailedCount =_userlogRepository.GetLoginFailedTimes(userName,DateTime.Now);
                if (loginFailedCount >= 5)
                    throw new BaseResultException { Code = BaseCode.数据验证失败, ResultMessage = "今日密码连续错误5次，已限制登录" };

                if (user.Password != GeneratePassword(password))
                    throw new BaseResultException { Code = BaseCode.查无数据, ResultMessage = "用户密码错误，当天还剩 "+loginFailedCount+ "次" };

                // 记录成功日志
                var userLoginLog = new UserActivityLog
                {
                    UserID = user.ID,
                    UserName = user.UserName,
                    NickName = user.NickName,
                    ActivityType = "LOGIN_SUCCEED",
                    Comment = "登录成功"
                };
              _userlogRepository.Insert(userLoginLog);

                user.IsOnline = "Y";
                user.DateLastLogin = DateTime.Now;
                user= _userRepository.Update(user);
                return new BaseResult<User>
                {
                    Data=user
                };
            }
            catch (BaseResultException ex)
            {
                // 记录失败日志
                var userLoginLog = new UserActivityLog
                {
                    UserName = userName,
                    ActivityType = "LOGIN_FAILED",
                    Comment = ex.ResultMessage+" "+password
                };
                _userlogRepository.Insert(userLoginLog);

                return new BaseResult<User>
                {
                    IsSucceed=false
                };
            }                                     

        }

        #region Private

        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string GeneratePassword(string password)
        {
            return SecurityHelper.Md5(password, "UTF-8");
        }

        /// <summary>
        /// 生成密码的加密盐
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string GenerateValideCode(string password)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
