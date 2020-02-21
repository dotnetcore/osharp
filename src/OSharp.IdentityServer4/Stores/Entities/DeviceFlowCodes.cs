// -----------------------------------------------------------------------
//  <copyright file="DeviceFlowCodes.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 0:49</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using IdentityModel;

using IdentityServer4.Models;

using OSharp.Data;
using OSharp.Entity;


namespace OSharp.IdentityServer4.Entities
{
    /// <summary>
    /// 实体类：设备流代码
    /// </summary>
    [Description("设备流代码")]
    [TableNamePrefix("Id4")]
    public class DeviceFlowCodes : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 设备代码
        /// </summary>
        [DisplayName("设备代码"), StringLength(200), Required]
        public string DeviceCode { get; set; }

        /// <summary>
        /// 获取或设置 用户代码
        /// </summary>
        [DisplayName("用户代码"), StringLength(200), Required]
        public string UserCode { get; set; }

        /// <summary>
        /// 获取或设置 主题编号
        /// </summary>
        [DisplayName("主题编号"), StringLength(200)]
        public string SubjectId { get; set; }

        /// <summary>
        /// 获取或设置 客户端编号
        /// </summary>
        [DisplayName("客户端编号"), StringLength(200), Required]
        public string ClientId { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 获取或设置 过期时间
        /// </summary>
        [DisplayName("过期时间")]
        public DateTime Expiration { get; set; }

        /// <summary>
        /// 获取或设置 数据
        /// </summary>
        [DisplayName("数据"), StringLength(50000), Required]
        public string Data { get; set; }
    }
}