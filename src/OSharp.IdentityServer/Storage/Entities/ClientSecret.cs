// -----------------------------------------------------------------------
//  <copyright file="ClientSecret.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-18 23:39</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.IdentityServer.Storage.Entities
{
    /// <summary>
    /// 实体类：客户端密钥
    /// </summary>
    [Description("客户端密钥")]
    [TableNamePrefix("Id4")]
    public class ClientSecret : Secret
    {
        /// <summary>
        /// 获取或设置 客户端编号
        /// </summary>
        [DisplayName("客户端编号")]
        public int ClientId { get; set; }

        /// <summary>
        /// 获取或设置 所属客户端
        /// </summary>
        [DisplayName("所属客户端")]
        public virtual Client Client { get; set; }
    }
}