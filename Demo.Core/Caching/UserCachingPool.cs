using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Common;
using Demo.Core.Domain.Account;

namespace Demo.Core.Caching
{
    public class UserCachingPool
    {
        private readonly ICacheManager _cacheManager = null;

        public UserCachingPool(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 每次交互的时候做操作
        /// </summary> 
        public void Push(User user)
        {
            if (_cacheManager.IsSet(user.ID.ToString()))
            {
                _cacheManager.Set(user.ID.ToString(), user, 20);
                return;
            }
            User cacheUser = _cacheManager.Get<User>(user.ID.ToString());  
        }

        public void Pop(User user)
        {
            if (_cacheManager.IsSet(user.ID.ToString()))
            {
                _cacheManager.Remove(user.ID.ToString());
            }
        }
    }
}
