// -----------------------------------------------------------------------
//  <copyright file="UserDetail.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 12:34</last-date>
// -----------------------------------------------------------------------

using OSharp.Entity;


namespace OSharp.Demo.Identity.Entities
{
    /// <summary>
    /// 实体类：用户详细信息
    /// </summary>
    public class UserDetail : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 获取或设置 所属用户
        /// </summary>
        public virtual User User { get; set; }
    }
}