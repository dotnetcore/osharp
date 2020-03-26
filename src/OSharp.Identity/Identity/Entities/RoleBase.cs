// -----------------------------------------------------------------------
//  <copyright file="RoleBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-04 19:33</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Audits;
using OSharp.Entity;


namespace OSharp.Identity.Entities
{
    /// <summary>
    /// 角色信息基类
    /// </summary>
    /// <typeparam name="TKey">角色编号类型</typeparam>
    [TableNamePrefix("Identity")]
    public abstract class RoleBase<TKey> : EntityBase<TKey>, ICreatedTime, ILockable, ISoftDeletable
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 初始化一个<see cref="RoleBase{TRoleKey}"/>类型的新实例
        /// </summary>
        protected RoleBase()
        {
            CreatedTime = DateTime.Now;
        }

        /// <summary>
        /// 获取或设置 角色名称
        /// </summary>
        [Required, DisplayName("角色名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 标准化角色名称
        /// </summary>
        [Required, DisplayName("标准化角色名称"), AuditIgnore]
        public string NormalizedName { get; set; }

        /// <summary>
        /// 获取或设置 一个随机值，每当某个角色被保存到存储区时，该值将发生变化。
        /// </summary>
        [DisplayName("版本标识"), AuditIgnore]
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 获取或设置 角色描述
        /// </summary>
        [StringLength(512)]
        [DisplayName("角色描述")]
        public string Remark { get; set; }

        /// <summary>
        /// 获取或设置 是否管理员角色
        /// </summary>
        [DisplayName("是否管理员角色")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 获取或设置 是否默认角色，用户注册后拥有此角色
        /// </summary>
        [DisplayName("是否默认角色")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 获取或设置 是否系统角色
        /// </summary>
        [DisplayName("是否系统角色")]
        public bool IsSystem { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        [DisplayName("是否锁定")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 数据逻辑删除时间，为null表示正常数据，有值表示已逻辑删除，同时删除时间每次不同也能保证索引唯一性
        /// </summary>
        public DateTime? DeletedTime { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }

    }
}