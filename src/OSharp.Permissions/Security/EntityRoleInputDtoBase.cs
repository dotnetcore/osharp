// -----------------------------------------------------------------------
//  <copyright file="EntityRoleInputDtoBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-03 23:02</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Data;
using OSharp.Entity;
using OSharp.Filter;


namespace OSharp.Security
{
    /// <summary>
    /// 实体角色输入DTO基类
    /// </summary>
    /// <typeparam name="TRoleKey">角色编号类型</typeparam>
    public abstract class EntityRoleInputDtoBase<TRoleKey> : IInputDto<Guid>
    {
        /// <summary>
        /// 初始化一个<see cref="EntityRoleInputDtoBase{TRoleKey}"/>类型的新实例
        /// </summary>
        protected EntityRoleInputDtoBase()
        {
            FilterGroup = new FilterGroup();
        }

        private Guid _id;
        /// <summary>
        /// 获取或设置 主键，唯一标识
        /// </summary>
        public Guid Id
        {
            get { return _id; }
            set
            {
                if (value == Guid.Empty)
                {
                    value = CombGuid.NewGuid();
                }
                _id = value;
            }
        }

        /// <summary>
        /// 获取或设置 角色编号
        /// </summary>
        public TRoleKey RoleId { get; set; }

        /// <summary>
        /// 获取或设置 数据编号
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// 获取或设置 过滤条件组
        /// </summary>
        public FilterGroup FilterGroup { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }
    }
}