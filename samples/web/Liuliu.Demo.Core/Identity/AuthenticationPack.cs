// -----------------------------------------------------------------------
//  <copyright file="AuthenticationPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-02 21:21</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Identity.Entities;

using OSharp.Authentication;


namespace Liuliu.Demo.Identity
{
    /// <summary>
    /// 身份认证模块
    /// </summary>
    public class AuthenticationPack : AuthenticationPackBase<User, int>
    { }
}