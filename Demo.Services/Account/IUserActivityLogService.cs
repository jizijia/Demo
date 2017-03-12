using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core;
using Demo.Core.Domain.Account;

namespace Demo.Services.Account
{
    /// <summary>
    /// 用户活动日志服务
    /// </summary>
    public interface IUserActivityLogService
    {
        /// <summary>
        /// 查询用户活动日志
        /// </summary>
        /// <param name="userName">账户名,采用模糊查询</param>
        /// <param name="nickName">昵称，采用模糊查询</param>
        /// <param name="typeName">活动日志类型，完全匹配</param>
        /// <param name="createDateBegin">创建开始时间，采用大于等于</param>
        /// <param name="createDateEnd">创建结束时间, 采用小于等于</param>
        /// <returns></returns>
        PagedList<UserActivityLog> GetUserActivityLogs(string userName, string nickName, string typeName, DateTime? createDateBegin, DateTime? createDateEnd);

        /// <summary>
        /// 添加一条活动日志
        /// </summary>
        /// <param name="userActivityLog"></param>
        void Insert(UserActivityLog userActivityLog);

        /// <summary>
        /// 获取用户登录错误次数
        /// 逻辑:
        ///     确认时间范围, 先找出当天最后一次登录成功时间, 有则作为开始时间,否则默认为00:00(大于等于), 结束时间为 24:00(小于)
        ///     通过用户名和时间范围进行查找统计记录数
        /// </summary>
        /// <param name="userName">用户名, 完全匹配</param>
        /// <param name="currentDate">当前日期,精确到天</param>
        /// <returns></returns>
        int GetLoginFailedTimes(string userName, DateTime? currentDate);

        /// <summary>
        /// 获取最后一次登录的IP地址
        /// </summary>
        /// <param name="userName">用户名，完全匹配</param>
        /// <returns></returns>
        string GetLastLoginIP(string userName);
    }
}
