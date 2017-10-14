using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Extension
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static class TryConvertExtension
    {
        #region 转换为Boolean数值
        /// <summary>
        /// 转换为Boolean数值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object value, bool defaultValue = false)
        {
            if (value==null) return defaultValue;
            if (bool.TryParse(value.ToString(), out bool temp))
            {
                return temp;
            }
            return defaultValue;
        }
        #endregion

        #region 转换对象为数字
        /// <summary>
        /// 转换为整型数值
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">转换失败后默认值</param>
        /// <returns></returns>
        public static int ToInt(this object value, int defaultValue=-1)
        {
            if (value == null) return defaultValue;
            if (!int.TryParse(value.ToString(), out int iReturn))
            {
                return defaultValue;
            }
            return iReturn;
        }
        #endregion

        #region 转换为短整型数值
        /// <summary>
        /// 转换为短整型数值
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">转换失败后默认值</param>
        /// <returns></returns>
        public static Int16 ToShort(this object value, short defaultValue=-1)
        {

            if (value == null) return defaultValue;
            if (!Int16.TryParse(value.ToString(), out short iReturn))
            {
                return defaultValue;
            }
            return iReturn;
        }
        #endregion

        #region 转换为单精度数值
        /// <summary>
        /// 转换为单精度数值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Single ToFloat(this object value, float defaultValue=0.0f)
        {
            float f = 0.0f;
            if (value == null) return defaultValue;
            if (float.TryParse(value.ToString(), out f)) return f;
            return defaultValue;
        }
        #endregion

        #region 将对象转换为decimal类型
        /// <summary>
        /// 将对象转换为decimal类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object value, decimal defaultValue = 0)
        {
            if (value == null)
                return defaultValue;
            if (!decimal.TryParse(value.ToString(), out decimal dValue))
            {
                dValue = defaultValue;
            }
            return dValue;
        }
        #endregion

        #region 将指定的对象转换为DateTime类型
        /// <summary>
        /// 转换为日期时间类型(转换失败后返回默认值)
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value, DateTime defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!DateTime.TryParse(value, out DateTime dResult))
                dResult = defaultValue;
            return dResult;
        }
        /// <summary>
        /// 转换为指定格式的日期时间类型(转换失败后返回默认值)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value, string format, DateTime defaultValue)
        {
            if (value == null)
                return defaultValue;
            if (!DateTime.TryParseExact(value, 
                                        format, 
                                        System.Globalization.CultureInfo.InvariantCulture, 
                                        System.Globalization.DateTimeStyles.None, 
                                        out DateTime dResult))
                dResult = defaultValue;
            return dResult;
        }
        #endregion
    }
}
