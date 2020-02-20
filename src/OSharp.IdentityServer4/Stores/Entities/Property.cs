// -----------------------------------------------------------------------
//  <copyright file="Property.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 0:01</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.IdentityServer4.Entities
{
    /// <summary>
    /// 属性基类
    /// </summary>
    public abstract class Property : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 属性键
        /// </summary>
        [DisplayName("属性键"), StringLength(250), Required]
        public string Key { get; set; }

        /// <summary>
        /// 获取或设置 属性值
        /// </summary>
        [DisplayName("属性值"), StringLength(2000), Required]
        public string Value { get; set; }
    }
}