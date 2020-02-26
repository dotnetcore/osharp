// -----------------------------------------------------------------------
//  <copyright file="UserRoleBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-01-31 19:21</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.Identity.Entities
{
    /// <summary>
    /// 用户角色映射基类
    /// </summary>
    /// <typeparam name="TKey">用户角色映射编号类型</typeparam>
    /// <typeparam name="TUserKey">用户编号类型</typeparam>
    /// <typeparam name="TRoleKey">角色编号类型</typeparam>
    [TableNamePrefix("Identity")]
    public abstract class UserRoleBase<TKey, TUserKey, TRoleKey> : EntityBase<TKey>, ICreatedTime, ILockable, ISoftDeletable
        where TKey : IEquatable<TKey>
        where TUserKey : IEquatable<TUserKey>
        where TRoleKey : IEquatable<TRoleKey>
    {
        /// <summary>
        /// 初始化一个<see cref="UserRoleBase{TKey, TUserKey, TRoleKey}"/>类型的新实例
        /// </summary>
        protected UserRoleBase()
        {
            CreatedTime = DateTime.Now;
        }

        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        [DisplayName("用户编号")]
        public TUserKey UserId { get; set; }

        /// <summary>
        /// 获取或设置 角色编号
        /// </summary>
        [DisplayName("角色编号")]
        public TRoleKey RoleId { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定当前信息
        /// </summary>
        [DisplayName("是否锁定")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 数据逻辑删除时间，为null表示正常数据，有值表示已逻辑删除，同时删除时间每次不同也能保证索引唯一性
        /// </summary>
        public DateTime? DeletedTime { get; set; }
    }
}