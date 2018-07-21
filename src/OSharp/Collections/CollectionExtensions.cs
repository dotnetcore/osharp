using System.Collections.Generic;

using OSharp.Data;


namespace OSharp.Collections
{
    /// <summary>
    /// 集合扩展方法
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 如果不存在，添加项
        /// </summary>
        public static void AddIfNotExist<T>(this ICollection<T> collection, T value)
        {
            Check.NotNull(collection, nameof(collection));
            if (!collection.Contains(value))
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// 如果不为空，添加项
        /// </summary>
        public static void AddIfNotNull<T>(this ICollection<T> collection, T value) where T : class
        {
            Check.NotNull(collection, nameof(collection));
            if (value != null)
            {
                collection.Add(value);
            }
        }
    }
}
