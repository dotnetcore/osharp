// -----------------------------------------------------------------------
//  <copyright file="Organization.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace Liuliu.Demo.Identity.Entities
{
    /// <summary>
    /// 实体类：组织机构
    /// </summary>
    [Description("组织机构信息")]
    [TableNamePrefix("Identity")]
    public class Organization : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 名称
        /// </summary>
        [Required, DisplayName("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 描述
        /// </summary>
        [DisplayName("描述")]
        public string Remark { get; set; }

        /// <summary>
        /// 获取或设置 父组织机构
        /// </summary>
        [DisplayName("父组织机构编号")]
        public int? ParentId { get; set; }
    }
}