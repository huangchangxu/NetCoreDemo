using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Extension
{
    /// <summary>
    /// 集合扩展
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 判断集合是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }
        /// <summary>
        /// 判断数组是否为NULL或者空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this T[] collection)
        {
            return collection == null || collection.Length == 0;
        }
        /// <summary>
        /// 遍历并操作集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="action"></param>
        public static void Each<T>(this IEnumerable<T> values, Action<T> action)
        {
            if (values == null) return;

            foreach (var value in values)
            {
                action(value);
            }
        }
        /// <summary>
        /// 遍历并操作集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="action"></param>
        public static void Each<T>(this IEnumerable<T> values, Action<int, T> action)
        {
            if (values == null) return;

            var i = 0;
            foreach (var value in values)
            {
                action(i++, value);
            }
        }

        public static List<To> Map<To, From>(this IEnumerable<From> items, Func<From, To> converter)
        {
            if (items == null)
                return new List<To>();

            var list = new List<To>();
            foreach (var item in items)
            {
                list.Add(converter(item));
            }
            return list;
        }

        public static List<To> Map<To>(this System.Collections.IEnumerable items, Func<object, To> converter)
        {
            if (items == null)
                return new List<To>();

            var list = new List<To>();
            foreach (var item in items)
            {
                list.Add(converter(item));
            }
            return list;
        }

        public static List<object> ToObjects<T>(this IEnumerable<T> items)
        {
            var to = new List<object>();
            foreach (var item in items)
            {
                to.Add(item);
            }
            return to;
        }

        public static string FirstNonDefaultOrEmpty(this IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value)) return value;
            }
            return null;
        }

        public static T FirstNonDefault<T>(this IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                if (!Equals(value, default(T))) return value;
            }
            return default(T);
        }

        public static bool EquivalentTo(this byte[] bytes, byte[] other)
        {
            var compare = 0;
            for (var i = 0; i < other.Length; i++)
                compare |= other[i] ^ bytes[i];

            return compare == 0;
        }

        public static bool EquivalentTo<T>(this T[] array, T[] otherArray, Func<T, T, bool> comparer = null)
        {
            if (array == null || otherArray == null)
                return array == otherArray;

            if (array.Length != otherArray.Length)
                return false;

            if (comparer == null)
                comparer = (v1, v2) => v1.Equals(v2);

            for (var i = 0; i < array.Length; i++)
            {
                if (!comparer(array[i], otherArray[i]))
                    return false;
            }

            return true;
        }

        public static bool EquivalentTo<T>(this IEnumerable<T> thisList, IEnumerable<T> otherList, Func<T, T, bool> comparer = null)
        {
            if (comparer == null)
                comparer = (v1, v2) => v1.Equals(v2);

            if (thisList == null || otherList == null)
                return thisList == otherList;

            var otherEnum = otherList.GetEnumerator();
            foreach (var item in thisList)
            {
                if (!otherEnum.MoveNext()) return false;

                var thisIsDefault = Equals(item, default(T));
                var otherIsDefault = Equals(otherEnum.Current, default(T));
                if (thisIsDefault || otherIsDefault)
                {
                    return thisIsDefault && otherIsDefault;
                }

                if (!comparer(item, otherEnum.Current)) return false;
            }
            var hasNoMoreLeftAsWell = !otherEnum.MoveNext();
            return hasNoMoreLeftAsWell;
        }

        public static IEnumerable<T[]> BatchesOf<T>(this IEnumerable<T> sequence, int batchSize)
        {
            var batch = new List<T>(batchSize);
            foreach (var item in sequence)
            {
                batch.Add(item);
                if (batch.Count >= batchSize)
                {
                    yield return batch.ToArray();
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
            {
                yield return batch.ToArray();
                batch.Clear();
            }
        }

        public static Dictionary<TKey, T> ToSafeDictionary<T, TKey>(this IEnumerable<T> list, Func<T, TKey> expr)
        {
            var map = new Dictionary<TKey, T>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    map[expr(item)] = item;
                }
            }
            return map;
        }

        public static Dictionary<TKey, TValue> ToDictionary<T, TKey, TValue>(this IEnumerable<T> list, Func<T, KeyValuePair<TKey, TValue>> map)
        {
            var to = new Dictionary<TKey, TValue>();
            foreach (var item in list)
            {
                var entry = map(item);
                to[entry.Key] = entry.Value;
            }
            return to;
        }
        
    }
}
