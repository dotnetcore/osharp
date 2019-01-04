// -----------------------------------------------------------------------
//  <copyright file="IgnoreGenType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-06 13:07</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.CodeGenerator
{
    /// <summary>
    /// 标记要在CodeGenerator生成代码时忽略的类型
    /// </summary>
    public class IgnoreGenTypeAttribute : Attribute
    { }
}