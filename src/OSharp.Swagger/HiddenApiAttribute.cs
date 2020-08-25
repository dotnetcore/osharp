// -----------------------------------------------------------------------
//  <copyright file="HiddenApiAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-31 13:32</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Swagger
{
    /// <summary>
    /// 标记此过滤器，API将在Swagger界面中隐藏
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class HiddenApiAttribute : Attribute
    { }
}