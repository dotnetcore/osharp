// -----------------------------------------------------------------------
//  <copyright file="ApiScopeClaim.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-18 23:58</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.IdentityServer4.Entities
{
    /// <summary>
    /// 实体类：API作用域声明
    /// </summary>
    [Description("API作用域声明")]
    [TableNamePrefix("Id4")]
    public class ApiScopeClaim : UserClaim
    {
        /// <summary>
        /// 获取或设置 所属API作用域编号
        /// </summary>
        [DisplayName("API作用域编号")]
        public int ApiScopeId { get; set; }

        /// <summary>
        /// 获取或设置 所属API作用域
        /// </summary>
        [DisplayName("API作用域")]
        public virtual ApiScope ApiScope { get; set; }
    }
}