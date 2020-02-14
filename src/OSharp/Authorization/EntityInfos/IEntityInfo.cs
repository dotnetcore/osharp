// -----------------------------------------------------------------------
//  <copyright file="IEntityInfo.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:14</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Entity;


namespace OSharp.Authorization.EntityInfos
{
    /// <summary>
    /// 定义数据实体信息
    /// </summary>
    public interface IEntityInfo : IEntity<Guid>, IEntityHash
    {
        /// <summary>
        /// 获取或设置 实体名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 获取或设置 实体类型名称
        /// </summary>
        string TypeName { get; set; }

        /// <summary>
        /// 获取或设置 是否允许审计
        /// </summary>
        bool AuditEnabled { get; set; }

        /// <summary>
        /// 获取或设置 实体属性信息JSON字符串
        /// </summary>
        string PropertyJson { get; set; }

        /// <summary>
        /// 获取 实体属性信息
        /// </summary>
        EntityProperty[] Properties { get; }

        /// <summary>
        /// 从实体类型初始化实体信息
        /// </summary>
        /// <param name="entityType"></param>
        void FromType(Type entityType);
    }
}