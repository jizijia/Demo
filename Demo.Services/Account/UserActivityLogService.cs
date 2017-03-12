using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core;
using Demo.Core.Domain.Account;
using Demo.Core.Base;
using Demo.Core.Data;
using Demo.Core.Common;
using System.Net;

namespace Demo.Services.Account
{
    public class UserActivityLogService : IUserActivityLogService
    {
        private readonly IRepository<UserActivityLog> _userlogRepository;
        public UserActivityLogService(IRepository<UserActivityLog> userlogRepository)
        {
            _userlogRepository = userlogRepository;
        }

        public string GetLastLoginIP(string userName)
        {
            throw new NotImplementedException();
        }

        public int GetLoginFailedTimes(string userName, DateTime? currentDate)
        {
            if (!currentDate .HasValue)
                currentDate = DateTime.Now;
            
            var loginSucceedLog = _userlogRepository.Table.Where(x => x.UserName == userName && x.ActivityType == "LOGIN_SUCCEED" && x.DateCreated .Value.Date == currentDate.Value.Date).OrderByDescending(x => x.DateCreated).FirstOrDefault();

           return   _userlogRepository.Count(c => c.DateCreated >= loginSucceedLog.DateCreated && c.ActivityType == "LOGIN_FAILED");
        }

        public PagedList<UserActivityLog> GetUserActivityLogs(string userName, string nickName, string typeName, DateTime? createDateBegin, DateTime? createDateEnd)
        {
            throw new NotImplementedException();
        }

        public void Insert(UserActivityLog userActivityLog)
        {
            if (userActivityLog == null)
                throw new BaseResultException { Code = MessageCode.DATA_VALIDATION_FAILED, ResultMessage = "数据不能为空" };

            if (userActivityLog.UserID<1)
                throw new BaseResultException { Code = MessageCode.DATA_VALIDATION_FAILED, ResultMessage = "用户ID为空" };

            if (userActivityLog.ActivityType.IsNone())
                throw new BaseResultException { Code = MessageCode.DATA_VALIDATION_FAILED, ResultMessage = "类型为空" };

            userActivityLog.IpAddress = NetworkHelper.GetClientIP();
            userActivityLog.ReferrerUrl = NetworkHelper.ReferrerUrl();
            userActivityLog.PageUrl = NetworkHelper.PageUrl();

          _userlogRepository.Insert(userActivityLog);
        }
    }
}
