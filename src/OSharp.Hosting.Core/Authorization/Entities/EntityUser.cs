// -----------------------------------------------------------------------
//  <copyright file="EntityUser.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Hosting.Identity.Entities;

using OSharp.Authorization.Entities;
using OSharp.Authorization.EntityInfos;


namespace OSharp.Hosting.Authorization.Entities
{
    /// <summary>
    /// 实体：数据用户信息
    /// </summary>
    [Description("数据用户信息")]
    public class EntityUser : EntityUserBase<int>
    {
        /// <summary>
        /// 获取或设置 所属用户信息
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// 获取或设置 所属实体信息
        /// </summary>
        public virtual EntityInfo EntityInfo { get; set; }
    }
}