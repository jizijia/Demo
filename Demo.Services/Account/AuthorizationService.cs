using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Core.Data;
using Demo.Core.Caching;
using Demo.Core;
using Demo.Core.Infrastructure;
using Demo.Core.Domain;
using System.Web;
using Demo.Core.Common;
using Demo.Core.Domain.Account;

namespace Demo.Service
{
    public class AuthorizationService : Demo.Service.IAuthorizationService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly TimeSpan _expirationTimeSpan;
        private readonly HttpContextBase _httpContext;
        private readonly ICacheManager _cacheManager;
        private User _cachedUser;

        public AuthorizationService(IAuthorizationService FuntionResourceService,
            IUserService userService,
            IRoleService roleService,
            HttpContextBase httpContext,
            ICacheManager cacheManager)
        {
            _authorizationService = FuntionResourceService;
            _userService = userService;
            _roleService = roleService;
            _httpContext = httpContext;
            _expirationTimeSpan = FormsAuthentication.Timeout;
            _cacheManager = cacheManager;
        }

        //#region Authorization
        ///// <summary>
        ///// 权限验证(当前用户)
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="controller"></param>
        ///// <param name="action"></param>
        ///// <returns></returns>
        //public bool IsAuthored(int userId, string controller, string action)
        //{
        //    #region 更新最后活动时间
        //    User user = GetAuthenticatedUser();
        //    if (user == null)
        //    {
        //        return false;
        //    }
        //    user.DateLastActivity = DateTime.Now;
        //    List<string> list = new List<string>();
        //    list.Add("DateLastActiveity");
        //    _userService.UpdateUser(user);
        //    #endregion

        //    //// 对所有登录的用户都授权Home/Index的访问
        //    //if (controller.ToLower() == "home" && action.ToLower() == "index")
        //    //    return true;

        //    if (user.FuntionResources == null)
        //        return false;
        //    foreach (var item in user.FuntionResources)
        //    {
        //        if (item.Action.ToLower() == action.ToLower() && item.Controller.ToLower() == controller.ToLower())
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        ///// <summary>
        ///// 权限验证(当前用户)
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="controller"></param>
        ///// <param name="action"></param>
        ///// <returns></returns>
        //public bool IsAuthored(string controller, string action)
        //{
        //    User user = GetAuthenticatedUser();
        //    if (user == null)
        //    {
        //        return false;
        //    }
        //    return IsAuthored(user.ID, controller, action);
        //}
        ///// <summary>
        ///// 用户登录
        ///// </summary>
        ///// <param name="loginID"></param>
        ///// <param name="password"></param>
        ///// <param name="persistCookie"></param>
        ///// <returns></returns>
        //public bool Login(string userName, string password, out string message, bool persistCookie = true)
        //{
        //    bool isValidated = _userService.Validate(userName, password, out message);
        //    if (isValidated)
        //    {
        //        User user = _userService.GetUserByUserName(userName);
        //        System.Web.HttpContext.Current.Session[ContractConst.USER_SESSION_KEY] = user;
        //        List<Permission> tempContainer = LoadAccesControllerByUser(user);
        //        tempContainer.ForEach(p =>
        //        {
        //            if (!user.FuntionResources.Contains(p))
        //            {
        //                user.FuntionResources.Add(p);
        //            }
        //        });
        //        _cachedUser = user;
        //        _cacheManager.Set(user.UserName, user, (Int32)_expirationTimeSpan.TotalMinutes);

        //        _userService.UpdateUser(user);
        //        return true;
        //    }
        //    return false;
        //}

        //private List<Permission> LoadAccesControllerByUser(User user)
        //{
        //    if (user.UserName == "admin")
        //    {
        //        return _FuntionResourceService.GetAllFuntionResources();
        //    }

        //    #region 加载角色的菜单
        //    foreach (Role item in user.Roles)
        //    {
        //        foreach (Permission ac in item.FuntionResources)
        //        {
        //            if (!user.FuntionResources.Contains(ac))
        //            {
        //                user.FuntionResources.Add(ac);
        //            }
        //        }
        //    }
        //    #endregion

        //    #region 加载公共项
        //    _FuntionResourceService.GetPublicFuntionResourceList().ForEach(c =>
        //    {
        //        if (!user.FuntionResources.Contains(c))
        //        {
        //            user.FuntionResources.Add(c);
        //        }
        //    });
        //    #endregion

        //    #region 追溯所有父级别
        //    List<Permission> FuntionResources = _FuntionResourceService.GetAllFuntionResources();
        //    IDictionary<string, Permission> dic = new Dictionary<string, Permission>();
        //    FuntionResources.ForEach(c =>
        //    {
        //        dic.Add(c.Code, c);
        //    });
        //    List<Permission> tempContainer = new List<Permission>();
        //    user.FuntionResources.Each(c =>
        //    {
        //        if (c.Code.Length > 4)
        //        {
        //            ReviewFatherAccessContorllers(dic, c.Code.Substring(0, c.Code.Length - 5), ref tempContainer);
        //        }
        //    });

        //    #endregion
        //    return tempContainer;
        //}

        //private void ReviewFatherAccessContorllers(IDictionary<string, Permission> source, string parentCode, ref List<Permission> temp)
        //{
        //    if (source.ContainsKey(parentCode))
        //    {
        //        if (!temp.Contains(source[parentCode]))
        //        {
        //            temp.Add(source[parentCode]);
        //            if (parentCode.Length > 4)
        //            {
        //                ReviewFatherAccessContorllers(source, source[parentCode].Code.Substring(0, source[parentCode].Code.Length - 5), ref temp);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// 注销
        ///// </summary>
        //public void Logout()
        //{
        //    _cachedUser = null;
        //    FormsAuthentication.SignOut();
        //}

        //public User GetAuthenticatedUser()
        //{
        //    if (_cachedUser != null)
        //        return _cachedUser;

        //    if (_httpContext == null ||
        //        _httpContext.Request == null ||
        //        !_httpContext.Request.IsAuthenticated ||
        //        _httpContext.User == null ||
        //        !(_httpContext.User.Identity is FormsIdentity))
        //    {
        //        return null;
        //    }

        //    var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
        //    var customer = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);
        //    _cachedUser = customer;
        //    return _cachedUser;
        //}
        //public User GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        //{
        //    if (ticket == null)
        //        throw new ArgumentNullException("ticket");

        //    var username = ticket.UserData;

        //    if (String.IsNullOrWhiteSpace(username))
        //        return null;
        //    var user = _cachedUser;
        //    if (_cachedUser == null)
        //        user = _cacheManager.Get<User>(username);
        //    if (user == null)
        //    {
        //        user = _userService.GetUserByUserName(username);
        //        #region 加载权限
        //        List<Permission> tempContainer = LoadAccesControllerByUser(user);
        //        tempContainer.ForEach(p =>
        //        {
        //            if (!user.FuntionResources.Contains(p))
        //            {
        //                user.FuntionResources.Add(p);
        //            }
        //        });
        //        _cacheManager.Set(user.UserName, user, (Int32)_expirationTimeSpan.TotalMinutes);
        //        #endregion
        //    }
        //    _cachedUser = user;
        //    return user;
        //}
        //#endregion
    }
}
