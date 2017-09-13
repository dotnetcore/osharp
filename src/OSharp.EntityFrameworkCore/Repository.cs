// -----------------------------------------------------------------------
//  <copyright file="Repository.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-17 0:16</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Z.EntityFramework.Plus;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体数据存储操作类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体主键类型</typeparam>
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// 初始化一个<see cref="Repository{TEntity, TKey}"/>类型的新实例
        /// </summary>
        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _dbContext = (DbContext)unitOfWork.GetDbContext<TEntity, TKey>();
            _dbSet = _dbContext.Set<TEntity>();
            //DbContext = (IDbContext)_dbContext;
        }

        /// <summary>
        /// 获取 当前单元操作对象
        /// </summary>
        public IUnitOfWork UnitOfWork { get; }

        //public IDbContext DbContext { get; }

        #region 同步方法

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Insert(params TEntity[] entities)
        {
            Check.NotNull(entities, nameof(entities));
            _dbSet.AddRange(entities);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(params TEntity[] entities)
        {
            Check.NotNull(entities, nameof(entities));

            _dbSet.RemoveRange(entities);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 删除指定编号的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(TKey key)
        {
            CheckEntityKey(key, nameof(key));

            TEntity entity = _dbSet.Find(key);
            return Delete(entity);
        }

        /// <summary>
        /// 删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        public int DeleteBatch(Expression<Func<TEntity, bool>> predicate)
        {
            Check.NotNull(predicate, nameof(predicate));

            return _dbSet.Where(predicate).Delete();
        }

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <param name="entity">更新后的实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));

            _dbSet.Update(entity);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 批量更新所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件的谓语表达式</param>
        /// <param name="updateExpression">属性更新表达式</param>
        /// <returns>操作影响的行数</returns>
        public int UpdateBatch(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(predicate, nameof(predicate));
            Check.NotNull(updateExpression, nameof(updateExpression));

            return _dbSet.Where(predicate).Update(updateExpression);
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="id">编辑的实体标识</param>
        /// <returns>是否存在</returns>
        public bool CheckExists(Expression<Func<TEntity, bool>> predicate, TKey id = default(TKey))
        {
            Check.NotNull(predicate, nameof(predicate));

            TKey defaultId = default(TKey);
            var entity = _dbSet.Where(predicate).Select(m => new { m.Id }).FirstOrDefault();
            bool exists = (!typeof(TKey).IsValueType && id.Equals(null)) || id.Equals(defaultId)
                ? entity != null
                : entity != null && !entity.Id.Equals(id);
            return exists;
        }

        /// <summary>
        /// 查找指定主键的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>符合主键的实体，不存在时返回null</returns>
        public TEntity Get(TKey key)
        {
            CheckEntityKey(key, nameof(key));

            return _dbSet.Find(key);
        }

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>不跟踪数据更改（NoTracking）的查询数据源，并可附加过滤条件
        /// </summary>
        /// <param name="predicate">数据过滤表达式</param>
        /// <returns></returns>
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            if (predicate == null)
            {
                return query;
            }
            return query.Where(predicate);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includePropertySelectors)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            if (includePropertySelectors != null && includePropertySelectors.Length > 0)
            {
                foreach (Expression<Func<TEntity, object>> selector in includePropertySelectors)
                {
                    query = query.Include(selector);
                }
            }
            return query.AsNoTracking();
        }

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>跟踪数据更改（Tracking）的查询数据源，并可附加过滤条件
        /// </summary>
        /// <param name="predicate">数据过滤表达式</param>
        /// <returns></returns>
        public IQueryable<TEntity> TrackQuery(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            if (predicate == null)
            {
                return query;
            }
            return query.Where(predicate);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> TrackQuery(params Expression<Func<TEntity, object>>[] includePropertySelectors)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            if (includePropertySelectors != null && includePropertySelectors.Length > 0)
            {
                foreach (Expression<Func<TEntity, object>> selector in includePropertySelectors)
                {
                    query = query.Include(selector);
                }
            }
            return query;
        }

        #endregion

        #region 异步方法

        /// <summary>
        /// 异步插入实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> InsertAsync(params TEntity[] entities)
        {
            Check.NotNull(entities, nameof(entities));

            await _dbSet.AddRangeAsync(entities);
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> DeleteAsync(params TEntity[] entities)
        {
            Check.NotNull(entities, nameof(entities));

            _dbSet.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 异步删除指定编号的实体
        /// </summary>
        /// <param name="key">实体编号</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> DeleteAsync(TKey key)
        {
            CheckEntityKey(key, nameof(key));

            TEntity entity = await _dbSet.FindAsync(key);
            return await DeleteAsync(entity);
        }

        /// <summary>
        /// 异步删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> DeleteBatchAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Check.NotNull(predicate, nameof(predicate));

            return await _dbSet.Where(predicate).DeleteAsync();
        }

        /// <summary>
        /// 异步更新实体对象
        /// </summary>
        /// <param name="entity">更新后的实体对象</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> UpdateAsync(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));

            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 异步更新所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="updateExpression">实体更新表达式</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> UpdateBatchAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(predicate, nameof(predicate));
            Check.NotNull(updateExpression, nameof(updateExpression));

            return await _dbSet.Where(predicate).UpdateAsync(updateExpression);
        }

        /// <summary>
        /// 异步检查实体是否存在
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="id">编辑的实体标识</param>
        /// <returns>是否存在</returns>
        public async Task<bool> CheckExistsAsync(Expression<Func<TEntity, bool>> predicate, TKey id = default(TKey))
        {
            predicate.CheckNotNull(nameof(predicate));

            TKey defaultId = default(TKey);
            var entity = await _dbSet.Where(predicate).Select(m => new { m.Id }).FirstOrDefaultAsync();
            bool exists = (!(typeof(TKey).IsValueType) && id.Equals(null)) || id.Equals(defaultId)
                ? entity != null
                : entity != null && !entity.Id.Equals(id);
            return exists;
        }

        /// <summary>
        /// 异步查找指定主键的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>符合主键的实体，不存在时返回null</returns>
        public async Task<TEntity> GetAsync(TKey key)
        {
            CheckEntityKey(key, nameof(key));

            return await _dbSet.FindAsync(key);
        }

        #endregion

        #region 私有方法

        private static void CheckEntityKey(object key, string keyName)
        {
            key.CheckNotNull("key");
            keyName.CheckNotNull("keyName");

            Type type = key.GetType();
            if (type == typeof(int))
            {
                Check.GreaterThan((int)key, keyName, 0);
            }
            else if (type == typeof(string))
            {
                Check.NotNullOrEmpty((string)key, keyName);
            }
            else if (type == typeof(Guid))
            {
                ((Guid)key).CheckNotEmpty(keyName);
            }
        }

        private static string GetNameValue(object value)
        {
            dynamic obj = value;
            try
            {
                return obj.Name;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}