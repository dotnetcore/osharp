using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
