// -----------------------------------------------------------------------
//  <copyright file="RepositoryExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-12 20:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OSharp.Exceptions;

using Z.EntityFramework.Extensions;
using Z.EntityFramework.Plus;


namespace OSharp.Entity
{
    /// <summary>
    /// <see cref="Repository{TEntity,TKey}"/>扩展辅助操作
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// 批量删除指定实体，并在提交前执行拦截委托
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">主键类型</typeparam>
        /// <param name="repository">仓储</param>
        /// <param name="predicate">查询谓语表达式</param>
        /// <param name="interceptAction">删除提交前执行的拦截委托</param>
        /// <returns>操作影响的行数</returns>
        public static int UpdateBatchAndIntercept<TEntity, TKey>(this IRepository<TEntity, TKey> repository,
            Expression<Func<TEntity, bool>> predicate,
            Action<BatchDelete> interceptAction)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return repository.Query(predicate).Delete(interceptAction);
        }

        /// <summary>
        /// 批量删除指定实体，并在提交前执行拦截委托
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">主键类型</typeparam>
        /// <param name="repository">仓储</param>
        /// <param name="predicate">查询谓语表达式</param>
        /// <param name="interceptAction">删除提交前执行的拦截委托</param>
        /// <returns>操作影响的行数</returns>
        public static async Task<int> UpdateBatchAndInterceptAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository,
            Expression<Func<TEntity, bool>> predicate,
            Action<BatchDelete> interceptAction)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return await repository.Query(predicate).DeleteAsync(interceptAction);
        }

        /// <summary>
        /// 批量更新指定实体，并在提交前执行拦截委托
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">主键类型</typeparam>
        /// <param name="repository">仓储</param>
        /// <param name="predicate">查询谓语表达式</param>
        /// <param name="updateExpression">属性更新表达式</param>
        /// <param name="interceptAction">更新提交前执行的拦截委托</param>
        /// <returns>操作影响的行数</returns>
        public static int UpdateBatchAndIntercept<TEntity, TKey>(this IRepository<TEntity, TKey> repository,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TEntity>> updateExpression,
            Action<BatchUpdate> interceptAction)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return repository.Query(predicate).Update(updateExpression, interceptAction);
        }

        /// <summary>
        /// 批量更新指定实体，并在提交前执行拦截委托
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">主键类型</typeparam>
        /// <param name="repository">仓储</param>
        /// <param name="predicate">查询谓语表达式</param>
        /// <param name="updateExpression">属性更新表达式</param>
        /// <param name="interceptAction">更新提交前执行的拦截委托</param>
        /// <returns>操作影响的行数</returns>
        public static async Task<int> UpdateBatchAndInterceptAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TEntity>> updateExpression,
            Action<BatchUpdate> interceptAction)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return await repository.Query(predicate).UpdateAsync(updateExpression, interceptAction);
        }

        /// <summary>
        /// 执行SQL查询获取数据
        /// </summary>
        public static IEnumerable<TEntity> FromSql<TEntity, TKey>(this IRepository<TEntity, TKey> repository, string sql, params object[] parameters)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            IDbContext dbContext = repository.DbContext;
            if (!(dbContext is DbContext context))
            {
                throw new OsharpException($"参数dbContext类型为 {dbContext.GetType()} ，不能转换为 DbContext");
            }

            return context.Set<TEntity>().FromSqlRaw(sql, parameters);
        }
    }
}