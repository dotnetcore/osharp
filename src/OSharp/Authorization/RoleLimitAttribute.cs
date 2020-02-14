// -----------------------------------------------------------------------
//  <copyright file="RoleLimitAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-15 2:45</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Authorization
{
    /// <summary>
    /// 指定功能只允许特定角色可以访问
    /// </summary>
    public class RoleLimitAttribute : Attribute
    { }
}