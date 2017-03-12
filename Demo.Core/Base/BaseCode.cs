using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Base
{
    public enum BaseCode
    {
        //代码错误
        服务器错误 = 500,

        //用户登录
        登录异常重新登录 = 10001,
        权限不足 = 10002,

        //数据验证
        数据验证失败 = 10101,
        数据异常 = 10102,
        查无数据 = 10103
    }
}
