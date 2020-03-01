// -----------------------------------------------------------------------
//  <copyright file="ApiResourceProperty.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 0:03</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.IdentityServer.Storage.Entities
{
    /// <summary>
    /// 实体类：API资源属性
    /// </summary>
    [Description("API资源属性")]
    [TableNamePrefix("Id4")]
    public class ApiResourceProperty : Property
    {
        /// <summary>
        /// 获取或设置 所属API资源编号
        /// </summary>
        [DisplayName("API资源编号")]
        public int ApiResourceId { get; set; }

        /// <summary>
        /// 获取或设置 所属API资源
        /// </summary>
        [DisplayName("API资源")]
        public virtual ApiResource ApiResource { get; set; }
    }
}