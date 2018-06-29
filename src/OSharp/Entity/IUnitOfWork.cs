// -----------------------------------------------------------------------
//  <copyright file="IUnitOfWork.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-16 22:29</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Entity
{
    /// <summary>
    /// 业务单元操作接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 获取 事务是否已提交
        /// </summary>
        bool HasCommited { get; }

        /// <summary>
        /// 获取指定数据上下文类型<typeparamref name="TEntity"/>的实例
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns><typeparamref name="TEntity"/>所属上下文类的实例</returns>
        IDbContext GetDbContext<TEntity, TKey>() where TEntity : IEntity<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        void Commit();
    }
}