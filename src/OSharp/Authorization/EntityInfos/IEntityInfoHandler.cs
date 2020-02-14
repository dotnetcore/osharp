// -----------------------------------------------------------------------
//  <copyright file="IEntityInfoHandler.cs" company="OSharp开源团队">
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
    /// 实体信息处理器
    /// </summary>
    public interface IEntityInfoHandler
    {
        /// <summary>
        /// 从程序集中刷新实体信息（实现了<see cref="IEntity{TKey}"/>接口的实体类）
        /// </summary>
        void Initialize();

        /// <summary>
        /// 查找指定实体类型的实体信息
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns>实体信息</returns>
        IEntityInfo GetEntityInfo(Type type);

        /// <summary>
        /// 查找指定实体类型的实体信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns>实体信息</returns>
        IEntityInfo GetEntityInfo<TEntity, TKey>() where TEntity : IEntity<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// 刷新实体信息缓存
        /// </summary>
        void RefreshCache();
    }
}