// -----------------------------------------------------------------------
//  <copyright file="PublicInstanceMethodInfoFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-15 2:38</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;


namespace OSharp.Reflection
{
    /// <summary>
    /// 公共实例方法查找器
    /// </summary>
    public class PublicInstanceMethodInfoFinder : IMethodInfoFinder
    {
        /// <summary>
        /// 查找指定条件的项
        /// </summary>
        /// <param name="type">要查找的类型</param>
        /// <param name="predicate">筛选条件</param>
        /// <returns></returns>
        public MethodInfo[] Find(Type type, Func<MethodInfo, bool> predicate)
        {
            return FindAll(type).Where(predicate).ToArray();
        }

        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <param name="type">要查找的类型</param>
        /// <returns></returns>
        public MethodInfo[] FindAll(Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }
    }
}