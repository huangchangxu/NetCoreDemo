using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Responses
{
    public class ApiCommonResponseDto<T>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="code">状态码{处理成功默认为200}</param>
        /// <param name="msg">状态码对应的文字描述</param>
        public ApiCommonResponseDto(long code=200,string msg = "")
        {
            responseHeader = new ApiResponseHeader
            {
                code = code,
                message = msg
            };
        }
        /// <summary>
        /// 响应描述
        /// </summary>
        public ApiResponseHeader responseHeader { get; set; }
        /// <summary>
        /// 响应数据
        /// </summary>
        public T data { get; set; }
    }

    /// <summary>
    /// api响应头
    /// </summary>
    public sealed class ApiResponseHeader
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public long time => Tools.CommonTools.GetTimeStamp();

        public string message { get; set; }
        /// <summary>
        /// 响应状态码
        /// </summary>
        public long code { get; set; }

        public string version => "1.0.0";
    }
}
