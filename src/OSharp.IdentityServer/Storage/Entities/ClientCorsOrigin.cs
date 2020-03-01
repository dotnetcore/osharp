// -----------------------------------------------------------------------
//  <copyright file="ClientCorsOrigin.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 0:24</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.IdentityServer.Storage.Entities
{
    /// <summary>
    /// 实体类：客户端跨域来源
    /// </summary>
    [Description("客户端跨域来源")]
    [TableNamePrefix("Id4")]
    public class ClientCorsOrigin : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 来源
        /// </summary>
        [DisplayName("来源"), StringLength(200), Required]
        public string Origin { get; set; }

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