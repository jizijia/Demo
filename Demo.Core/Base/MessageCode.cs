using Demo.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Base
{
    public class MessageCode
    {
        // 服务器错误
        [Description("服务器错误")]
        public const string SERVER_ERROR_500 = "500";


        // 用户登录
        [Description("登录异常重新登录")]
        public const string ACCOUNT_UNAUTHORIZED = "10001";

        [Description("权限不足")]
        public const string ACCOUNT_PERMISSION_DENIED = "10002";

        //数据验证

        [Description("数据验证失败")]
        public const string DATA_VALIDATION_FAILED = "10101";

        [Description("数据异常")]
        public const string DATA_EXCEPTION = "10102";

        [Description("查无数据")]
        public const string DATA_NOT_FOUND= "10103";
    }
}
