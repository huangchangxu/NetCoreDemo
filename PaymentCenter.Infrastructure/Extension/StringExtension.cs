using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PaymentCenter.Infrastructure.Extension
{
    /// <summary>
    /// String 扩展
    /// </summary>
    public static class StringExtension
    {
        public static bool IsNullOrWhiteSpace(this string inputStr)
        {
            return string.IsNullOrWhiteSpace(inputStr);
        }

        public static bool IsNullOrEmpty(this string inputStr)
        {
            return string.IsNullOrEmpty(inputStr);
        }

        public static string Format(this string inputStr, params object[] param)
        {
            return string.Format(inputStr, param);
        }

        public static string RegexReplace(this string inputStr, string pattern, string replacement)
        {
            return (inputStr.IsNullOrEmpty()) ? inputStr : Regex.Replace(inputStr, pattern, replacement);
        }

        public static string Sub(this string inputStr, int length)
        {
            if (inputStr.IsNullOrEmpty())
            {
                return string.Empty;
            }
            return ((inputStr.Length >= length) ? inputStr.Substring(0, length) : inputStr);
        }

        public static string TryReplace(this string inputStr, string oldStr, string newStr)
        {
            return (inputStr.IsNullOrEmpty() ? inputStr : inputStr.Replace(oldStr, newStr));
        }
        /// <summary>
        /// 解析JSON字符串生成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(this string jsonStr)where T:class
        {
            try
            {
                return Tools.JsonTool.DeserializeJsonToObject<T>(jsonStr);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
