// -----------------------------------------------------------------------
//  <copyright file="AuditIgnoreAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-26 15:06</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Audits
{
    /// <summary>
    /// 标记在审计中忽略的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AuditIgnoreAttribute : Attribute
    { }
}