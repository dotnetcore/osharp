// -----------------------------------------------------------------------
//  <copyright file="MethodCallInfo.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-29 19:56</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Wpf.Hubs.Reflection
{
    /// <summary>
    /// 方法调用信息
    /// </summary>
    public class MethodCallInfo
    {
        /// <summary> 反射法名称 </summary>
        public string MethodName { get; set; }

        /// <summary> 参数值 </summary>
        public Type[] ParameterTypes { get; set; }
    }
}