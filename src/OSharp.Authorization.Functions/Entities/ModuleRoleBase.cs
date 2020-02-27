// -----------------------------------------------------------------------
//  <copyright file="ModuleRoleBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-15 13:37</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.Authorization.Entities
{
    /// <summary>
    /// 模块角色信息基类
    /// </summary>
    /// <typeparam name="TModuleKey">模块编号</typeparam>
    /// <typeparam name="TRoleKey">角色编号</typeparam>
    [TableNamePrefix("Auth")]
    public abstract class ModuleRoleBase<TModuleKey, TRoleKey> : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 模块编号
        /// </summary>
        [DisplayName("模块编号")]
        public TModuleKey ModuleId { get; set; }

        /// <summary>
        /// 获取或设置 角色编号
        /// </summary>
        [DisplayName("角色编号")]
        public TRoleKey RoleId { get; set; }
    }
}