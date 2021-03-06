﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PaymentCenter.Infrastructure.Tools
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public sealed class CommonTools
    {
        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        /// <summary>
        /// 根据特性Model数据是否合法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public static bool ModelValidation(object data, out string errormsg)
        {
            var validationContext = new ValidationContext(data);
            var resultValidation = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(data, validationContext, resultValidation, true);
            errormsg = string.Join("|", resultValidation.Select(x => x.ErrorMessage));
            return isValid;
        }
    }
}
