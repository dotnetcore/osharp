// -----------------------------------------------------------------------
//  <copyright file="ClientScope.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 23:34</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.IdentityServer4.Entities
{
    /// <summary>
    /// 实体类：客户端作用域
    /// </summary>
    [Description("客户端作用域")]
    [TableNamePrefix("Id4")]
    public class ClientScope : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 作用域
        /// </summary>
        [DisplayName("作用域"), StringLength(200), Required]
        public string Scope { get; set; }

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