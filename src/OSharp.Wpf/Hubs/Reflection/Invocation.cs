// -----------------------------------------------------------------------
//  <copyright file="Invocation.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-29 19:45</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Wpf.Hubs.Reflection
{
    /// <summary>
    /// 方法名称和参数值的容器
    /// </summary>
    public class Invocation
    {
        /// <summary>
        /// 获取或设置 要调用的方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 获取或设置 调用参数
        /// </summary>
        public object[] Parameters { get; set; }

        /// <summary>
        /// 获取或设置 返回类型，void时为null
        /// </summary>
        public Type ReturnType { get; set; }
    }
}