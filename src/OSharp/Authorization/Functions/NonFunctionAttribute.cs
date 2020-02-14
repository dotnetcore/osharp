// -----------------------------------------------------------------------
//  <copyright file="NonFunctionAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:14</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Authorization.Functions
{
    /// <summary>
    /// 标注当前Action不作为Function信息进行收集
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NonFunctionAttribute : Attribute
    { }
}