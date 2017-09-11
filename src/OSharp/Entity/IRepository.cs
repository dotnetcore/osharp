// -----------------------------------------------------------------------
//  <copyright file="IRepository.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-16 22:49</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using OSharp.Dependency;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义实体仓储模型的数据标准操作
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public interface IRepository<TEntity, in TKey> : IScopeDependency
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 获取 当前单元操作对象
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 获取 当前的数据上下文
        /// </summary>
        IDbContext DbContext { get; }

        #region 同步方法

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        int Insert(params TEntity[] entities);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        int Delete(params TEntity[] entities);

        /// <summary>
        /// 删除指定编号的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>操作影响的行数</returns>
        int Delete(TKey key);

        /// <summary>
        /// 删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        int Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <param name="entity">更新后的实体对象</param>
        /// <returns>操作影响的行数</returns>
        int Update(TEntity entity);

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="id">编辑的实体标识</param>
        /// <returns>是否存在</returns>
        bool CheckExists(Expression<Func<TEntity, bool>> predicate, TKey id = default(TKey));

        /// <summary>
        /// 查找指定主键的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>符合主键的实体，不存在时返回null</returns>
        TEntity Get(TKey key);

        /// <summary>
        /// 查找指定键的实体，支持复合键
        /// </summary>
        /// <param name="ids">键值</param>
        /// <returns>符合主键的实体，不存在时返回null</returns>
        TEntity GetById(params object[] ids);

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>不跟踪数据更改（NoTracking）的查询数据源
        /// </summary>
        /// <param name="propertySelectors">要Include操作的属性表达式</param>
        /// <returns></returns>
        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>跟踪数据更改（Tracking）的查询数据源
        /// </summary>
        /// <param name="propertySelectors">要Include操作的属性表达式</param>
        /// <returns></returns>
        IQueryable<TEntity> TrackQuery(params Expression<Func<TEntity, object>>[] propertySelectors);

        #endregion

        #region 异步方法

        /// <summary>
        /// 异步插入实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        Task<int> InsertAsync(params TEntity[] entities);

        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        Task<int> DeleteAsync(params TEntity[] entities);

        /// <summary>
        /// 异步删除指定编号的实体
        /// </summary>
        /// <param name="key">实体编号</param>
        /// <returns>操作影响的行数</returns>
        Task<int> DeleteAsync(TKey key);

        /// <summary>
        /// 异步删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步更新实体对象
        /// </summary>
        /// <param name="entity">更新后的实体对象</param>
        /// <returns>操作影响的行数</returns>
        Task<int> UpdateAsync(TEntity entity);

        /// <summary>
        /// 异步检查实体是否存在
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="id">编辑的实体标识</param>
        /// <returns>是否存在</returns>
        Task<bool> CheckExistsAsync(Expression<Func<TEntity, bool>> predicate, TKey id = default(TKey));

        /// <summary>
        /// 异步查找指定主键的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>符合主键的实体，不存在时返回null</returns>
        Task<TEntity> GetAsync(TKey key);

        #endregion
    }
}