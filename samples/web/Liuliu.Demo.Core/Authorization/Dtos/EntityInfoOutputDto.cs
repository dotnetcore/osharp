// -----------------------------------------------------------------------
//  <copyright file="EntityInfoOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Authorization.EntityInfos;
using OSharp.Entity;
using OSharp.Mapping;


namespace Liuliu.Demo.Authorization.Dtos
{
    /// <summary>
    /// 输出DTO:实体信息
    /// </summary>
    [MapFrom(typeof(EntityInfo))]
    public class EntityInfoOutputDto : IOutputDto, IDataAuthEnabled
    {
        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 实体名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 实体类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 获取或设置 是否启用数据日志
        /// </summary>
        public bool AuditEnabled { get; set; }

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