using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Common;

namespace Demo.Core.Base
{
    /// <summary>
    /// 基础返回数据
    /// </summary>
    public class BaseResult
    {
        /// <summary>
        /// 验证结果
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 验证代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 验证信息
        /// </summary>
        public string Message { get; set; }

        public BaseResult()
        {
            IsSuccess = true;
        }
        public BaseResult(object data)
        {
            Data = data;
            IsSuccess = true;
        }
    }
    /// <summary>
    /// 基础返回数据
    /// </summary>
    public class BaseResult<T>
    {
        /// <summary>
        /// 验证结果
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// 验证代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 验证信息
        /// </summary>
        public string Message { get; set; }

        public BaseResult()
        {
            IsSucceed = true;
        }
        public BaseResult(T data)
        {
            Data = data;
            IsSucceed = true;
        }
    }

    public class BaseResultException : Exception
    {
        /// <summary>
        /// 验证代码
        /// </summary>
        public BaseCode Code { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// 验证信息
        /// </summary>
        public string ResultMessage { get; set; }

        public BaseResult ToBaseResult()
        {
            return new BaseResult
            {
                IsSuccess = false,
                Data = Result,
                Message = ResultMessage.IsNone() ? Code.ToString() : ResultMessage
            };
        }
    }
}
