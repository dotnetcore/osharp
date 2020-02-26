// -----------------------------------------------------------------------
//  <copyright file="EntityRoleBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-15 13:37</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Filter;


namespace OSharp.Authorization.Entities
{
    /// <summary>
    /// 数据角色实体基类
    /// </summary>
    [TableNamePrefix("Auth")]
    public abstract class EntityRoleBase<TRoleKey> : EntityBase<Guid>, ILockable, ICreatedTime
    {
        /// <summary>
        /// 获取或设置 角色编号
        /// </summary>
        [DisplayName("角色编号")]
        public TRoleKey RoleId { get; set; }

        /// <summary>
        /// 获取或设置 数据实体编号
        /// </summary>
        [DisplayName("数据编号")]
        public Guid EntityId { get; set; }

        /// <summary>
        /// 获取或设置 数据权限操作
        /// </summary>
        [DisplayName("数据权限操作")]
        public DataAuthOperation Operation { get; set; }

        /// <summary>
        /// 获取或设置 过滤条件组Json字符串
        /// </summary>
        [DisplayName("过滤条件组Json字符串")]
        public string FilterGroupJson { get; set; }

        /// <summary>
        /// 获取 过滤条件组信息
        /// </summary>
        [NotMapped]
        public FilterGroup FilterGroup
        {
            get
            {
                if (FilterGroupJson.IsNullOrEmpty())
                {
                    return null;
                }
                return FilterGroupJson.FromJsonString<FilterGroup>();
            }
        }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        [DisplayName("是否锁定")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreatedTime { get; set; }
    }
}