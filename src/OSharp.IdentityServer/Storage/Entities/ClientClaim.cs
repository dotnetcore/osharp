// -----------------------------------------------------------------------
//  <copyright file="ClientClaim.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 0:20</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.IdentityServer.Storage.Entities
{
    /// <summary>
    /// 实体类：客户端声明
    /// </summary>
    [Description("客户端声明")]
    [TableNamePrefix("Id4")]
    public class ClientClaim : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 声明类型
        /// </summary>
        [DisplayName("声明类型"), Required, StringLength(250)]
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置 声明值
        /// </summary>
        [DisplayName("声明值"), Required, StringLength(1000)]
        public string Value { get; set; }

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