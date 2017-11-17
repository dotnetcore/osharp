// -----------------------------------------------------------------------
//  <copyright file="UserRole.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-06 7:57</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Identity;


namespace OSharp.Demo.Identity.Entities
{
    /// <summary>
    /// 实体类：用户角色信息
    /// </summary>
    [Description("用户角色信息")]
    public class UserRole : UserRoleBase<int, int>
    { }
}