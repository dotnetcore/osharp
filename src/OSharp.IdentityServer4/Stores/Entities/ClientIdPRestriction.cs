// -----------------------------------------------------------------------
//  <copyright file="ClientIdPRestriction.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 0:31</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.IdentityServer4.Entities
{
    /// <summary>
    /// 实体类：客户端Id提供程序限制
    /// </summary>
    [TableNamePrefix("Id4")]
    public class ClientIdPRestriction : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 提供程序
        /// </summary>
        [DisplayName("提供程序"), StringLength(200), Required]
        public string Provider { get; set; }

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