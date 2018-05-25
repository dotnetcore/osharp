// -----------------------------------------------------------------------
//  <copyright file="JwtClaimTypes.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-26 0:07</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Security.JwtBearer
{
    /// <summary>
    /// Jwt声明类型
    /// </summary>
    public struct JwtClaimTypes
    {
        public const string Id = "id";
        public const string Name = "name";
        public const string Email = "email";
        public const string NickName = "nick-name";
        public const string PhoneNumber = "phone";
        public const string Roles = "roles";
        public const string IsAdmin = "is-admin";
        public const string SecurityStamp = "security-stamp";
    }
}