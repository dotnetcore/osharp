// -----------------------------------------------------------------------
//  <copyright file="ClientPostLogoutRedirectUri.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 0:33</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.IdentityServer.Storage.Entities
{
    /// <summary>
    /// 实体类：客户端登出跳转URL
    /// </summary>
    [Description("客户端登出跳转URL")]
    [TableNamePrefix("Id4")]
    public class ClientPostLogoutRedirectUri : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 跳转URL
        /// </summary>
        [DisplayName("跳转URL"), StringLength(2000), Required]
        public string PostLogoutRedirectUri { get; set; }

        /// <summary>
        /// 获取或设置 所属客户端编号
        /// </summary>
        [DisplayName("客户端编号")]
        public int ClientId { get; set; }

        /// <summary>
        /// 获取或设置 所属客户端
        /// </summary>
        [DisplayName("客户端")]
        public virtual Client Client { get; set; }
    }
}