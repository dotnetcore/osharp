// -----------------------------------------------------------------------
//  <copyright file="ModuleUser.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-18 14:56</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Security;


namespace OSharp.Demo.Security.Entities
{
    /// <summary>
    /// 实体类：模块用户信息
    /// </summary>
    [Description("用户模块信息")]
    public class ModuleUser : ModuleUserBase<int, int>
    { }
}