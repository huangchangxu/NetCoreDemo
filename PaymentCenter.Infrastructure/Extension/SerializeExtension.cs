using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PaymentCenter.Infrastructure.Extension
{
    /// <summary>
    /// 序列化扩展
    /// </summary>
    public static class SerializeExtension
    {
        /// <summary>
        /// 将byte数据转换为UTF8字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string DeserializeUtf8(this byte[] stream)
        {
            return ((stream == null) ? null : Encoding.UTF8.GetString(stream));
        }
        /// <summary>
        /// 将json字符串反序列化成指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string jsonStr)
        {
            return (jsonStr.IsNullOrEmpty() ? default(T) : JsonConvert.DeserializeObject<T>(jsonStr));
        }
        /// <summary>
        /// 将字符串转换为UTF8编码 byte数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] SerializeUtf8(this string str)
        {
            return ((str == null) ? null : Encoding.UTF8.GetBytes(str));
        }
        /// <summary>
        /// 将对象序列化为json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ignoreNull"></param>
        /// <returns></returns>
        public static string ToJson(this object obj, [Optional, DefaultParameterValue(false)]bool ignoreNull)
        {
            if (obj.IsNull())
            {
                return string.Empty;
            }
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include
            };
            return JsonConvert.SerializeObject(obj, Formatting.None, settings);
        }
        /// <summary>
        /// 将byte数组反序列化为指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T BinaryDeserialize<T>(this byte[] stream)
        {
            if (stream == null)
                return default(T);

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(stream))
            {
                var result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
        /// <summary>
        /// 将指定对象序列为byte数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] BinarySerialize(this object obj)
        {
            if (obj == null)
                return null;

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                var data = memoryStream.ToArray();
                return data;
            }
        }

    }
}
