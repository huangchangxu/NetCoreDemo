using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PaymentCenter.Infrastructure.Extension
{
    /// <summary>
    /// Attribute扩展
    /// </summary>
    public static class AttributeExtension
    {
        public static T GetAttribute<T>(this object obj) where T : class
        {
            return obj.GetType().GetAttribute<T>();
        }

        public static T GetAttribute<T>(this Type type) where T : class
        {
            Attribute customAttribute = type.GetCustomAttribute(typeof(T));
            if (customAttribute.IsNotNull())
            {
                return (customAttribute as T);
            }
            return default(T);
        }
    }
}
