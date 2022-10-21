// -----------------------------------------------------------------------
//  <copyright file="ComparisonHelper.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-18 16:03</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;


namespace OSharp.Collections
{
    /// <summary>
    /// 比较器辅助类，用于快速创建<see cref="IComparer{T}"/>接口的实例
    /// </summary>
    /// <example>
    /// var comparer1 = Comparison[Person].CreateComparer(p => p.ID); var comparer2 = Comparison[Person].CreateComparer(p => p.Name); var comparer3 = Comparison[Person].CreateComparer(p => p.Birthday.Year)
    /// </example>
    /// <typeparam name="T">要比较的类型</typeparam>
    public static class ComparisonHelper<T>
    {
        /// <summary>
        /// 创建指定对比委托<paramref name="keySelector"/>的实例
        /// </summary>
        public static IComparer<T> CreateComparer<TV>(Func<T, TV> keySelector)
        {
            return new CommonComparer<TV>(keySelector);
        }

        /// <summary>
        /// 创建指定对比委托<paramref name="keySelector"/>与结果二次比较器<paramref name="comparer"/>的实例
        /// </summary>
        public static IComparer<T> CreateComparer<TV>(Func<T, TV> keySelector, IComparer<TV> comparer)
        {
            return new CommonComparer<TV>(keySelector, comparer);
        }
        
        private class CommonComparer<TV> : IComparer<T>
        {
            private readonly IComparer<TV> _comparer;
            private readonly Func<T, TV> _keySelector;

            public CommonComparer(Func<T, TV> keySelector, IComparer<TV> comparer)
            {
                _keySelector = keySelector;
                _comparer = comparer;
            }

            public CommonComparer(Func<T, TV> keySelector)
                : this(keySelector, Comparer<TV>.Default)
            { }
            
            public int Compare(T x, T y)
            {
                return _comparer.Compare(_keySelector(x), _keySelector(y));
            }
            
        }
        
    }
}