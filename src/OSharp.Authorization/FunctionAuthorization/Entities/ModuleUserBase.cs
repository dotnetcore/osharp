// -----------------------------------------------------------------------
//  <copyright file="ModuleUserBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-11 1:28</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.Authorization.Entities
{
    /// <summary>
    /// 模块用户信息基类
    /// </summary>
    /// <typeparam name="TModuleKey">模块编号</typeparam>
    /// <typeparam name="TUserKey">用户编号</typeparam>
    [TableNamePrefix("Auth")]
    public abstract class ModuleUserBase<TModuleKey, TUserKey> : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 模块编号
        /// </summary>
        [DisplayName("模块编号")]
        public TModuleKey ModuleId { get; set; }

        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        [DisplayName("用户编号")]
        public TUserKey UserId { get; set; }

        /// <summary>
        /// 获取或设置 是否禁用。用户与模块的关系，默认从角色继承得来，如禁用，则禁用从角色继承的权限（黑名单），如不禁用，则相当于用户额外添加角色继承以外的权限（白名单）
        /// </summary>
        public bool Disabled { get; set; }
    }
}