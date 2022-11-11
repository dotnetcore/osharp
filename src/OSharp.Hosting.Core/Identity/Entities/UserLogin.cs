// -----------------------------------------------------------------------
//  <copyright file="UserLogin.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Hosting.Identity.Entities;

/// <summary>
/// 实体类：用户登录及其提供程序
/// </summary>
[Description("用户登录及其提供程序")]
public class UserLogin : UserLoginBase<long, long>
{
    /// <summary>
    /// 获取或设置 所属用户信息
    /// </summary>
    public virtual User User { get; set; }
}
