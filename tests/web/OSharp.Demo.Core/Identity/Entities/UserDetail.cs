// -----------------------------------------------------------------------
//  <copyright file="UserDetail.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-06 7:59</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.Demo.Identity.Entities
{
    /// <summary>
    /// 实体类：用户详细信息
    /// </summary>
    [Description("用户详细信息")]
    public class UserDetail : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 地址
        /// </summary>
        [DisplayName("地址")]
        public string Address { get; set; }

        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        [DisplayName("用户编号")]
        public virtual int UserId { get; set; }
    }
}