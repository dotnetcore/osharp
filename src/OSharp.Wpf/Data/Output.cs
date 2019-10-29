// -----------------------------------------------------------------------
//  <copyright file="Output.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-29 10:44</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Wpf.Data
{
    /// <summary>
    /// 信息输出类 
    /// </summary>
    public static class Output
    {
        /// <summary>
        /// 获取或设置 状态栏输出委托
        /// </summary>
        public static Action<string> StatusBar = msg => throw new InvalidOperationException("Output is not initialized");
    }
}