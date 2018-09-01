// -----------------------------------------------------------------------
//  <copyright file="IUnitOfWorkManager.cs" company="柳柳软件">
//      Copyright (c) 2016-2018 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-31 13:21</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Entity
{
    /// <summary>
    /// 工作单元管理器，统一处理各个工作单元的事务
    /// </summary>
    public interface IUnitOfWorkManager : IDisposable
    {
        /// <summary>
        /// 获取 事务是否已提交
        /// </summary>
        bool HasCommited { get; }

        /// <summary>
        /// 获取指定实体所在的工作单元对象
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns>工作单元对象</returns>
        IUnitOfWork GetUnitOfWork<TEntity, TKey>() where TEntity : IEntity<TKey>;

        /// <summary>
        /// 获取指定实体所在的工作单元对象
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>工作单元对象</returns>
        IUnitOfWork GetUnitOfWork(Type entityType);

        /// <summary>
        /// 提交所有工作单元的事务更改
        /// </summary>
        void Commit();
    }
}