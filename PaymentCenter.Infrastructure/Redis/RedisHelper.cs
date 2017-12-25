using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.Linq;
using PaymentCenter.Infrastructure.Extension;
using System.Threading.Tasks;

namespace PaymentCenter.Infrastructure.Redis
{
    public class RedisHelper
    {
        /// <summary>
        /// 创建redis链接
        /// </summary>
        /// <param name="dbNum"></param>
        /// <param name="defaultKey"></param>
        /// <param name="redisUri"></param>
        /// <returns></returns>
        public static IDatabase GetDatabase(int dbNum = 0, string redisUri = "")
        {
            var config = new RedisClientConfigurations { Url = redisUri };
            var client = RedisClient.GetRedisClient(config);
            if (client.IsConnected)
            {
                return client.GetDatabase(db: dbNum);
            }
            else
            {
                throw new ArgumentException("RedisHelper.client", $"Redis未能成功连接。连接配置为{config.ToJson()}");
            }
        }
        /// <summary>
        /// 创建redis订阅链接
        /// </summary>
        /// <param name="redisUri"></param>
        /// <returns></returns>
        public static ISubscriber GetSubscriber(string redisUri = "")
        {
            var config = new RedisClientConfigurations { Url = redisUri };
            var client = RedisClient.GetRedisClient(config);
            if (client.IsConnected)
            {
                client.PreserveAsyncOrder = config.PreserveAsyncOrder;
                return client.GetSubscriber();
            }
            else
            {
                throw new ArgumentException("RedisHelper.client", $"Redis订阅未能成功连接。连接配置为{config.ToJson()}");
            }
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private static IEnumerable<string> ConvertStrings<T>(IEnumerable<T> list) where T : struct
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            return list.Select(x => x.ToString());
        }

        #region String 操作

        /// <summary>
        /// 设置 key 并保存字符串（如果 key 已存在，则覆盖值）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <param name="dbNum"></param>
        /// <param name="redisUrl"></param>
        /// <returns></returns>
        public static bool StringSet(string redisKey, string redisValue, TimeSpan? expiry = null, int dbNum = 0, string redisUrl = "")
        {
            var db = GetDatabase(dbNum, redisUrl);
            return db.StringSet(redisKey, redisValue, expiry);
        }

        /// <summary>
        /// 保存多个 Key-value
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <param name="dbNum"></param>
        /// <param name="redisUrl"></param>
        /// <returns></returns>
        public static bool StringSet(IEnumerable<KeyValuePair<RedisKey, RedisValue>> keyValuePairs, int dbNum = 0, string redisUrl = "")
        {
            var db = GetDatabase(dbNum, redisUrl);
            return db.StringSet(keyValuePairs.ToArray());
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <param name="dbNum"></param>
        /// <param name="redisUrl"></param>
        /// <returns></returns>
        public static string StringGet(string redisKey, TimeSpan? expiry = null, int dbNum = 0, string redisUrl = "")
        {
            var db = GetDatabase(dbNum, redisUrl);
            return db.StringGet(redisKey);
        }

        /// <summary>
        /// 存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <param name="dbNum"></param>
        /// <param name="redisUrl"></param>
        /// <returns></returns>
        public static bool StringSet<T>(string redisKey, T redisValue, TimeSpan? expiry = null, int dbNum = 0, string redisUrl = "")
        {
            var db = GetDatabase(dbNum, redisUrl);
            return db.StringSet(redisKey, redisValue.ToJson(), expiry);
        }

        /// <summary>
        /// 获取一个对象（会进行反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <param name="dbNum"></param>
        /// <param name="redisUrl"></param>
        /// <returns></returns>
        public static T StringGet<T>(string redisKey, TimeSpan? expiry = null, int dbNum = 0, string redisUrl = "")
        {
            var value = GetDatabase(dbNum, redisUrl).StringGet(redisKey);
            if (value.HasValue)
                return value.ToString().FromJson<T>();
            else
                return default(T);
        }

        #region async

        /// <summary>
        /// 保存一个字符串值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(string redisKey, string redisValue, TimeSpan? expiry = null, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).StringSetAsync(redisKey, redisValue, expiry);
        }

        /// <summary>
        /// 保存一组字符串值
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(IEnumerable<KeyValuePair<RedisKey, RedisValue>> keyValuePairs, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).StringSetAsync(keyValuePairs.ToArray());
        }

        /// <summary>
        /// 获取单个值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<string> StringGetAsync(string redisKey, string redisValue, TimeSpan? expiry = null, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).StringGetAsync(redisKey);
        }

        /// <summary>
        /// 存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync<T>(string redisKey, T redisValue, TimeSpan? expiry = null, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).StringSetAsync(redisKey, redisValue.ToJson(), expiry);
        }

        /// <summary>
        /// 获取一个对象（会进行反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<T> StringGetAsync<T>(string redisKey, TimeSpan? expiry = null, int dbNum = 0, string redisUrl = "")
        {
            var value = await GetDatabase(dbNum, redisUrl).StringGetAsync(redisKey);
            return value.ToString().FromJson<T>();
        }

        #endregion async

        #endregion String 操作

        #region Hash 操作

        /// <summary>
        /// 判断该字段是否存在 hash 中
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static bool HashExists(string redisKey, string hashField, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).HashExists(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static bool HashDelete(string redisKey, string hashField, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).HashDelete(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public static long HashDelete(string redisKey, IEnumerable<string> hashFields, int dbNum = 0, string redisUrl = "")
        {
            var fields = hashFields.Select(x => (RedisValue)x);
            return GetDatabase(dbNum, redisUrl).HashDelete(redisKey, fields.ToArray());
        }

        /// <summary>
        /// 在 hash 设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HashSet(string redisKey, string hashField, string value, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).HashSet(redisKey, hashField, value);
        }

        /// <summary>
        /// 在 hash 中设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        public static void HashSet(string redisKey, IEnumerable<KeyValuePair<string, string>> hashFields, int dbNum = 0, string redisUrl = "")
        {
            var entries = hashFields.Select(x => new HashEntry(x.Key, x.Value));
            GetDatabase(dbNum, redisUrl).HashSet(redisKey, entries.ToArray());
        }

        /// <summary>
        /// 在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static string HashGet(string redisKey, string hashField, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).HashGet(redisKey, hashField);
        }

        /// <summary>
        /// 在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public static IEnumerable<string> HashGet(string redisKey, IEnumerable<string> hashFields, int dbNum = 0, string redisUrl = "")
        {
            var fields = hashFields.Select(x => (RedisValue)x);
            return ConvertStrings(GetDatabase(dbNum, redisUrl).HashGet(redisKey, fields.ToArray()));
        }

        /// <summary>
        /// 从 hash 返回所有的字段值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IEnumerable<string> HashKeys(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return ConvertStrings(GetDatabase(dbNum, redisUrl).HashKeys(redisKey));
        }

        /// <summary>
        /// 返回 hash 中的所有值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IEnumerable<string> HashValues(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return ConvertStrings(GetDatabase(dbNum, redisUrl).HashValues(redisKey));
        }

        /// <summary>
        /// 在 hash 设定值（序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static bool HashSet<T>(string redisKey, string hashField, T redisValue, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).HashSet(redisKey, hashField, redisValue.ToJson());
        }

        /// <summary>
        /// 在 hash 中获取值（反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static T HashGet<T>(string redisKey, string hashField, int dbNum = 0, string redisUrl = "")
        {
            var value = GetDatabase(dbNum, redisUrl).HashGet(redisKey, hashField);
            if (value.HasValue)
                return value.ToString().FromJson<T>();
            else
                return default(T);
        }

        #region async

        /// <summary>
        /// 判断该字段是否存在 hash 中
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static async Task<bool> HashExistsAsync(string redisKey, string hashField, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).HashExistsAsync(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static async Task<bool> HashDeleteAsync(string redisKey, string hashField, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).HashDeleteAsync(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public static async Task<long> HashDeleteAsync(string redisKey, IEnumerable<string> hashFields, int dbNum = 0, string redisUrl = "")
        {
            var fields = hashFields.Select(x => (RedisValue)x);
            return await GetDatabase(dbNum, redisUrl).HashDeleteAsync(redisKey, fields.ToArray());
        }

        /// <summary>
        /// 在 hash 设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<bool> HashSetAsync(string redisKey, string hashField, string value, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).HashSetAsync(redisKey, hashField, value);
        }

        /// <summary>
        /// 在 hash 中设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        public static async Task HashSetAsync(string redisKey, IEnumerable<KeyValuePair<string, string>> hashFields, int dbNum = 0, string redisUrl = "")
        {
            var entries = hashFields.Select(x => new HashEntry(x.Key, x.Value));
            await GetDatabase(dbNum, redisUrl).HashSetAsync(redisKey, entries.ToArray());
        }

        /// <summary>
        /// 在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static async Task<string> HashGetAsync(string redisKey, string hashField, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).HashGetAsync(redisKey, hashField);
        }

        /// <summary>
        /// 在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> HashGetAsync(string redisKey, IEnumerable<string> hashFields,
            string value, int dbNum = 0, string redisUrl = "")
        {
            var fields = hashFields.Select(x => (RedisValue)x);
            return ConvertStrings(await GetDatabase(dbNum, redisUrl).HashGetAsync(redisKey, fields.ToArray()));
        }

        /// <summary>
        /// 从 hash 返回所有的字段值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> HashKeysAsync(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return ConvertStrings(await GetDatabase(dbNum, redisUrl).HashKeysAsync(redisKey));
        }

        /// <summary>
        /// 返回 hash 中的所有值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> HashValuesAsync(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return ConvertStrings(await GetDatabase(dbNum, redisUrl).HashValuesAsync(redisKey));
        }

        /// <summary>
        /// 在 hash 设定值（序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<bool> HashSetAsync<T>(string redisKey, string hashField, T value, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).HashSetAsync(redisKey, hashField, value.ToJson());
        }

        /// <summary>
        /// 在 hash 中获取值（反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static async Task<T> HashGetAsync<T>(string redisKey, string hashField, int dbNum = 0, string redisUrl = "")
        {
            var value= (await GetDatabase(dbNum, redisUrl).HashGetAsync(redisKey, hashField));
            if (value.HasValue)
                return value.ToString().FromJson<T>();
            else
                return default(T);
        }

        #endregion async

        #endregion Hash 操作

        #region List 操作

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static string ListLeftPop(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).ListLeftPop(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static string ListRightPop(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).ListRightPop(redisKey);
        }

        /// <summary>
        /// 移除列表指定键上与该值相同的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListRemove(string redisKey, string redisValue, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).ListRemove(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListRightPush(string redisKey, string redisValue, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).ListRightPush(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListLeftPush(string redisKey, string redisValue, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).ListLeftPush(redisKey, redisValue);
        }

        /// <summary>
        /// 返回列表上该键的长度，如果不存在，返回 0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static long ListLength(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).ListLength(redisKey);
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static IEnumerable<string> ListRange(string redisKey, long start = 0L, long stop = -1L, int dbNum = 0, string redisUrl = "")
        {
            return ConvertStrings(GetDatabase(dbNum, redisUrl).ListRange(redisKey, start, stop));
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static T ListLeftPop<T>(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            var value=(GetDatabase(dbNum, redisUrl).ListLeftPop(redisKey));
            if (value.HasValue)
                return value.ToString().FromJson<T>();
            else
                return default(T);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static T ListRightPop<T>(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            var value=(GetDatabase(dbNum, redisUrl).ListRightPop(redisKey));
            if (value.HasValue)
                return value.ToString().FromJson<T>();
            else
                return default(T);
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListRightPush<T>(string redisKey, T redisValue, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).ListRightPush(redisKey, redisValue.ToJson());
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListLeftPush<T>(string redisKey, T redisValue, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).ListLeftPush(redisKey, redisValue.ToJson());
        }

        #region List-async

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<string> ListLeftPopAsync(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).ListLeftPopAsync(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<string> ListRightPopAsync(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).ListRightPopAsync(redisKey);
        }

        /// <summary>
        /// 移除列表指定键上与该值相同的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static async Task<long> ListRemoveAsync(string redisKey, string redisValue, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).ListRemoveAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static async Task<long> ListRightPushAsync(string redisKey, string redisValue, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).ListRightPushAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static async Task<long> ListLeftPushAsync(string redisKey, string redisValue, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).ListLeftPushAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 返回列表上该键的长度，如果不存在，返回 0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<long> ListLengthAsync(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).ListLengthAsync(redisKey);
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> ListRangeAsync(string redisKey, long start = 0L, long stop = -1L, int dbNum = 0, string redisUrl = "")
        {
            var query = await GetDatabase(dbNum, redisUrl).ListRangeAsync(redisKey, start, stop);
            return query.Select(x => x.ToString());
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<T> ListLeftPopAsync<T>(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            var value = (await GetDatabase(dbNum, redisUrl).ListLeftPopAsync(redisKey));
            if (value.HasValue)
                return value.ToString().FromJson<T>();
            else
                return default(T);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<T> ListRightPopAsync<T>(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            var value = (await GetDatabase(dbNum, redisUrl).ListRightPopAsync(redisKey));
            if (value.HasValue)
                return value.ToString().FromJson<T>();
            else
                return default(T);
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static async Task<long> ListRightPushAsync<T>(string redisKey, T redisValue, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).ListRightPushAsync(redisKey, redisValue.ToJson());
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static async Task<long> ListLeftPushAsync<T>(string redisKey, T redisValue, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).ListLeftPushAsync(redisKey, redisValue.ToJson());
        }

        #endregion List-async

        #endregion List 操作

        #region SortedSet 操作

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool SortedSetAdd(string redisKey, string member, double score, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).SortedSetAdd(redisKey, member, score);
        }

        /// <summary>
        /// 在有序集合中返回指定范围的元素，默认情况下从低到高。
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static IEnumerable<string> SortedSetRangeByRank(string redisKey, long start = 0L, long stop = -1L,
            Order order = Order.Ascending, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).SortedSetRangeByRank(redisKey, start, stop, order).Select(x => x.ToString());
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static long SortedSetLength(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).SortedSetLength(redisKey);
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="memebr"></param>
        /// <returns></returns>
        public static bool SortedSetLength(string redisKey, string memebr, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).SortedSetRemove(redisKey, memebr);
        }

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool SortedSetAdd<T>(string redisKey, T member, double score, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).SortedSetAdd(redisKey, member.ToJson(), score);
        }

        /// <summary>
        /// 增量的得分排序的集合中的成员存储键值键按增量
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double SortedSetIncrement(string redisKey, string member, double value = 1, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).SortedSetIncrement(redisKey, member, value);
        }

        #region SortedSet-Async

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static async Task<bool> SortedSetAddAsync(string redisKey, string member, double score, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).SortedSetAddAsync(redisKey, member, score);
        }

        /// <summary>
        /// 在有序集合中返回指定范围的元素，默认情况下从低到高。
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> SortedSetRangeByRankAsync(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return ConvertStrings(await GetDatabase(dbNum, redisUrl).SortedSetRangeByRankAsync(redisKey));
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<long> SortedSetLengthAsync(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).SortedSetLengthAsync(redisKey);
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="memebr"></param>
        /// <returns></returns>
        public static async Task<bool> SortedSetRemoveAsync(string redisKey, string memebr, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).SortedSetRemoveAsync(redisKey, memebr);
        }

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static async Task<bool> SortedSetAddAsync<T>(string redisKey, T member, double score, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).SortedSetAddAsync(redisKey, member.ToJson(), score);
        }

        /// <summary>
        /// 增量的得分排序的集合中的成员存储键值键按增量
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task<double> SortedSetIncrementAsync(string redisKey, string member, double value = 1, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).SortedSetIncrementAsync(redisKey, member, value);
        }

        #endregion SortedSet-Async

        #endregion SortedSet 操作

        #region key 操作

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static bool KeyDelete(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).KeyDelete(redisKey);
        }

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public static long KeyDelete(IEnumerable<string> redisKeys, int dbNum = 0, string redisUrl = "")
        {
            var keys = redisKeys.Select(x => (RedisKey)x);
            return GetDatabase(dbNum, redisUrl).KeyDelete(keys.ToArray());
        }

        /// <summary>
        /// 校验 Key 是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static bool KeyExists(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).KeyExists(redisKey);
        }

        /// <summary>
        /// 重命名 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisNewKey"></param>
        /// <returns></returns>
        public static bool KeyRename(string redisKey, string redisNewKey, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).KeyRename(redisKey, redisNewKey);
        }

        /// <summary>
        /// 设置 Key 的时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool KeyExpire(string redisKey, TimeSpan? expiry, int dbNum = 0, string redisUrl = "")
        {
            return GetDatabase(dbNum, redisUrl).KeyExpire(redisKey, expiry);
        }

        #region key-async

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<bool> KeyDeleteAsync(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).KeyDeleteAsync(redisKey);
        }

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public static async Task<long> KeyDeleteAsync(IEnumerable<string> redisKeys, int dbNum = 0, string redisUrl = "")
        {
            var keys = redisKeys.Select(x => (RedisKey)x);
            return await GetDatabase(dbNum, redisUrl).KeyDeleteAsync(keys.ToArray());
        }

        /// <summary>
        /// 校验 Key 是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<bool> KeyExistsAsync(string redisKey, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).KeyExistsAsync(redisKey);
        }

        /// <summary>
        /// 重命名 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisNewKey"></param>
        /// <returns></returns>
        public static async Task<bool> KeyRenameAsync(string redisKey, string redisNewKey, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).KeyRenameAsync(redisKey, redisNewKey);
        }

        /// <summary>
        /// 设置 Key 的时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> KeyExpireAsync(string redisKey, TimeSpan? expiry, int dbNum = 0, string redisUrl = "")
        {
            return await GetDatabase(dbNum, redisUrl).KeyExpireAsync(redisKey, expiry);
        }

        #endregion key-async

        #endregion key 操作

        #region Redis发布订阅
        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="subChannel"></param>
        public static void RedisSub(string subChannel, Action<string, string> onMessage = null, string redisUrl = "")
        {
            var _sub = GetSubscriber(redisUrl);
            _sub.Subscribe(subChannel, (channel, message) =>
            {
                onMessage?.Invoke(channel, message);
            });
        }
        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static long RedisPub<T>(string channel, T msg, string redisUrl = "")
        {
            var _sub = GetSubscriber(redisUrl);
            return _sub.Publish(channel, msg.ToJson());
        }
        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="channel"></param>
        public static void Unsubscribe(string channel, string redisUrl = "")
        {
            var _sub = GetSubscriber(redisUrl);
            _sub.Unsubscribe(channel);
        }
        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        public static void UnsubscribeAll(string redisUrl = "")
        {
            var _sub = GetSubscriber(redisUrl);
            _sub.UnsubscribeAll();
        }
        #endregion
    }
}
