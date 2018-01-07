using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Log
{
    /// <summary>
    /// 普通日志实体
    /// </summary>
    public class LogInfoDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime => DateTime.Now;
        /// <summary>
        /// 日子级别
        /// </summary>
        public LogLevel Level { get; set; }
        /// <summary>
        /// 写入日志的类名称
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// 写入日志的项目名称
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 必须是一个json字符串，可以为空
        /// </summary>
        public string OtherAttribute { get; set; }

        /// <summary>
        /// 放入到es中的类型，默认是infolog,程序中处理
        /// </summary>
        public string Estype { get; set; }
    }
}
