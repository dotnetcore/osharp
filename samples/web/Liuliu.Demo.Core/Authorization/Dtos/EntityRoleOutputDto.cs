// -----------------------------------------------------------------------
//  <copyright file="EntityRoleOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-04 15:12</last-date>
// -----------------------------------------------------------------------

using System;

using Liuliu.Demo.Authorization.Entities;

using OSharp.Authorization;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Filter;
using OSharp.Mapping;


namespace Liuliu.Demo.Authorization.Dtos
{
    /// <summary>
    /// 输出DTO: 角色数据权限
    /// </summary>
    [MapFrom(typeof(EntityRole))]
    public class EntityRoleOutputDto : IOutputDto, IDataAuthEnabled
    {
        /// <summary>
        /// 初始化一个<see cref="EntityRoleOutputDto"/>类型的新实例
        /// </summary>
        public EntityRoleOutputDto()
        { }

        /// <summary>
        /// 初始化一个<see cref="EntityRoleOutputDto"/>类型的新实例
        /// </summary>
        public EntityRoleOutputDto(EntityRole entityRole)
        {
            Id = entityRole.Id;
            RoleId = entityRole.RoleId;
            EntityId = entityRole.EntityId;
            IsLocked = entityRole.IsLocked;
            Operation = entityRole.Operation;
            CreatedTime = entityRole.CreatedTime;
            FilterGroup = entityRole.FilterGroupJson.FromJsonString<FilterGroup>();
        }

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 角色编号
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 获取或设置 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 获取或设置 数据编号
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// 获取或设置 业务实体名称
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 获取或设置 业务实体类型
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 获取或设置 数据权限操作
        /// </summary>
        public DataAuthOperation Operation { get; set; }

        /// <summary>
        /// 获取或设置 过滤条件组
        /// </summary>
        public FilterGroup FilterGroup { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        #region Implementation of IDataAuthEnabled

        /// <summary>
        /// 获取或设置 是否可更新的数据权限状态
        /// </summary>
        public bool Updatable { get; set; }

        /// <summary>
        /// 获取或设置 是否可删除的数据权限状态
        /// </summary>
        public bool Deletable { get; set; }

        #endregion
    }
}