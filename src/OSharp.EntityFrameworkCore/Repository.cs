// -----------------------------------------------------------------------
//  <copyright file="Repository.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-15 19:20</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Authorization;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Entity.KeyGenerate;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Filter;
using OSharp.Identity;
using OSharp.Mapping;
using OSharp.Threading;

using Z.EntityFramework.Plus;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体数据存储操作类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体主键类型</typeparam>
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        private readonly ILogger _logger;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        private readonly IPrincipal _principal;

        /// <summary>
        /// 初始化一个<see cref="Repository{TEntity, TKey}"/>类型的新实例
        /// </summary>
        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbContext = serviceProvider.GetDbContext<TEntity, TKey>();
            _dbSet = ((DbContext)_dbContext).Set<TEntity>();
            _logger = serviceProvider.GetLogger<Repository<TEntity, TKey>>();
            _cancellationTokenProvider = serviceProvider.GetService<ICancellationTokenProvider>();
            _principal = serviceProvider.GetService<IPrincipal>();
        }

        /// <summary>
        /// 获取 数据上下文
        /// </summary>
        public IDbContext DbContext => _dbContext;

        /// <summary>
        /// 获取 <typeparamref name="TEntity"/>不跟踪数据更改（NoTracking）的查询数据源
        /// </summary>
        public virtual IQueryable<TEntity> Entities
        {
            get
            {
                Expression<Func<TEntity, bool>> dataFilterExp = GetDataFilter(DataAuthOperation.Read);
                return _dbSet.AsQueryable().AsNoTracking().Where(dataFilterExp);
            }
        }

        /// <summary>
        /// 获取 <typeparamref name="TEntity"/>跟踪数据更改（Tracking）的查询数据源
        /// </summary>
        public virtual IQueryable<TEntity> TrackEntities
        {
            get
            {
                Expression<Func<TEntity, bool>> dataFilterExp = GetDataFilter(DataAuthOperation.Read);
                return _dbSet.AsQueryable().Where(dataFilterExp);
            }
        }

        #region 同步方法

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public virtual int Insert(params TEntity[] entities)
        {
            Check.NotNull(entities, nameof(entities));
            entities = CheckInsert(entities);
            _dbSet.AddRange(entities);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 插入或更新实体
        /// </summary>
        /// <param name="entities">要处理的实体</param>
        /// <param name="existingFunc">实体是否存在的判断委托</param>
        /// <returns>操作影响的行数</returns>
        public virtual int InsertOrUpdate(TEntity[] entities, Func<TEntity, Expression<Func<TEntity, bool>>> existingFunc = null)
        {
            Check.NotNull(entities, nameof(entities));
            foreach (TEntity entity in entities)
            {
                Expression<Func<TEntity, bool>> exp = existingFunc == null
                    ? m => m.Id.Equals(entity.Id)
                    : existingFunc(entity);
                if (!_dbSet.Any(exp))
                {
                    CheckInsert(entity);
                    _dbSet.Add(entity);
                }
                else
                {
                    CheckUpdate(entity);
                    ((DbContext)_dbContext).Update<TEntity, TKey>(entity);
                }
            }

            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 以DTO为载体批量插入实体
        /// </summary>
        /// <typeparam name="TInputDto">添加DTO类型</typeparam>
        /// <param name="dtos">添加DTO信息集合</param>
        /// <param name="checkAction">添加信息合法性检查委托</param>
        /// <param name="updateFunc">由DTO到实体的转换委托</param>
        /// <returns>业务操作结果</returns>
        public virtual OperationResult Insert<TInputDto>(ICollection<TInputDto> dtos,
            Action<TInputDto> checkAction = null,
            Func<TInputDto, TEntity, TEntity> updateFunc = null) where TInputDto : IInputDto<TKey>
        {
            Check.NotNull(dtos, nameof(dtos));
            List<string> names = new List<string>();
            foreach (TInputDto dto in dtos)
            {
                try
                {
                    if (checkAction != null)
                    {
                        checkAction(dto);
                    }
                    TEntity entity = dto.MapTo<TEntity>();
                    if (updateFunc != null)
                    {
                        entity = updateFunc(dto, entity);
                    }
                    entity = CheckInsert(entity)[0];
                    _dbSet.Add(entity);
                }
                catch (OsharpException e)
                {
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                names.AddIfNotNull(GetNameValue(dto));
            }
            int count = _dbContext.SaveChanges();
            return count > 0
                ? new OperationResult(OperationResultType.Success,
                    names.Count > 0
                        ? $"信息 {names.ExpandAndToString()} 添加成功"
                        : $"{dtos.Count}个信息添加成功")
                : new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public virtual int Delete(params TEntity[] entities)
        {
            Check.NotNull(entities, nameof(entities));

            DeleteInternal(entities);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 删除指定编号的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>操作影响的行数</returns>
        public virtual int Delete(TKey key)
        {
            CheckEntityKey(key, nameof(key));

            TEntity entity = _dbSet.Find(key);
            return Delete(entity);
        }

        /// <summary>
        /// 以标识集合批量删除实体
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="checkAction">删除前置检查委托</param>
        /// <param name="deleteFunc">删除委托，用于删除关联信息</param>
        /// <returns>业务操作结果</returns>
        public virtual OperationResult Delete(ICollection<TKey> ids, Action<TEntity> checkAction = null, Func<TEntity, TEntity> deleteFunc = null)
        {
            Check.NotNull(ids, nameof(ids));
            List<string> names = new List<string>();
            foreach (TKey id in ids)
            {
                TEntity entity = _dbSet.Find(id);
                if (entity == null)
                {
                    continue;
                }
                try
                {
                    if (checkAction != null)
                    {
                        checkAction(entity);
                    }
                    if (deleteFunc != null)
                    {
                        entity = deleteFunc(entity);
                    }
                    DeleteInternal(entity);
                }
                catch (OsharpException e)
                {
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                names.AddIfNotNull(GetNameValue(entity));
            }
            int count = _dbContext.SaveChanges();
            return count > 0
                ? new OperationResult(OperationResultType.Success,
                    names.Count > 0
                        ? $"信息 {names.ExpandAndToString()} 删除成功"
                        : $"{ids.Count}个信息删除成功")
                : new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        public virtual int DeleteBatch(Expression<Func<TEntity, bool>> predicate)
        {
            Check.NotNull(predicate, nameof(predicate));
            // todo: 检测删除的数据权限

            ((DbContextBase)_dbContext).BeginOrUseTransaction();
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                // 逻辑删除
                TEntity[] entities = _dbSet.Where(predicate).ToArray();
                DeleteInternal(entities);
                return _dbContext.SaveChanges();
            }

            //物理删除
            return _dbSet.Where(predicate).Delete();
        }

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <param name="entities">更新后的实体对象</param>
        /// <returns>操作影响的行数</returns>
        public virtual int Update(params TEntity[] entities)
        {
            Check.NotNull(entities, nameof(entities));
            entities = CheckUpdate(entities);
            ((DbContext)_dbContext).Update<TEntity, TKey>(entities);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 以DTO为载体批量更新实体
        /// </summary>
        /// <typeparam name="TEditDto">更新DTO类型</typeparam>
        /// <param name="dtos">更新DTO信息集合</param>
        /// <param name="checkAction">更新信息合法性检查委托</param>
        /// <param name="updateFunc">由DTO到实体的转换委托</param>
        /// <returns>业务操作结果</returns>
        public virtual OperationResult Update<TEditDto>(ICollection<TEditDto> dtos,
            Action<TEditDto, TEntity> checkAction = null,
            Func<TEditDto, TEntity, TEntity> updateFunc = null) where TEditDto : IInputDto<TKey>
        {
            Check.NotNull(dtos, nameof(dtos));
            List<string> names = new List<string>();
            foreach (TEditDto dto in dtos)
            {
                try
                {
                    TEntity entity = _dbSet.Find(dto.Id);
                    if (entity == null)
                    {
                        return new OperationResult(OperationResultType.QueryNull);
                    }
                    if (checkAction != null)
                    {
                        checkAction(dto, entity);
                    }
                    entity = dto.MapTo(entity);
                    if (updateFunc != null)
                    {
                        entity = updateFunc(dto, entity);
                    }
                    entity = CheckUpdate(entity)[0];
                    ((DbContext)_dbContext).Update<TEntity, TKey>(entity);
                }
                catch (OsharpException e)
                {
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                names.AddIfNotNull(GetNameValue(dto));
            }
            int count = _dbContext.SaveChanges();
            return count > 0
                ? new OperationResult(OperationResultType.Success,
                    names.Count > 0
                        ? $"信息 {names.ExpandAndToString()} 更新成功"
                        : $"{dtos.Count}个信息更新成功")
                : new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 批量更新所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件的谓语表达式</param>
        /// <param name="updateExpression">属性更新表达式</param>
        /// <returns>操作影响的行数</returns>
        public virtual int UpdateBatch(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(predicate, nameof(predicate));
            Check.NotNull(updateExpression, nameof(updateExpression));

            ((DbContextBase)_dbContext).BeginOrUseTransaction();
            return _dbSet.Where(predicate).Update(updateExpression);
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="id">编辑的实体标识</param>
        /// <returns>是否存在</returns>
        public virtual bool CheckExists(Expression<Func<TEntity, bool>> predicate, TKey id = default)
        {
            Check.NotNull(predicate, nameof(predicate));

            TKey defaultId = default;
            var entity = _dbSet.Where(predicate).Select(m => new { m.Id }).FirstOrDefault();
            bool exists = !typeof(TKey).IsValueType && ReferenceEquals(id, null) || id.Equals(defaultId)
                ? entity != null
                : entity != null && !EntityBase<TKey>.IsKeyEqual(entity.Id, id);
            return exists;
        }

        /// <summary>
        /// 查找指定主键的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>符合主键的实体，不存在时返回null</returns>
        public virtual TEntity Get(TKey key)
        {
            CheckEntityKey(key, nameof(key));

            return _dbSet.Find(key);
        }

        /// <summary>
        /// 查找第一个符合条件的数据
        /// </summary>
        /// <param name="predicate">数据查询谓语表达式</param>
        /// <returns>符合条件的实体，不存在时返回null</returns>
        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            predicate.CheckNotNull("predicate");
            return GetFirst(predicate, true);
        }

        /// <summary>
        /// 查找第一个符合条件的数据
        /// </summary>
        /// <param name="predicate">数据查询谓语表达式</param>
        /// <param name="filterByDataAuth">是否使用数据权限过滤，数据权限一般用于存在用户实例的查询，系统查询不启用数据权限过滤</param>
        /// <returns>符合条件的实体，不存在时返回null</returns>
        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate, bool filterByDataAuth)
        {
            Check.NotNull(predicate, nameof(predicate));
            return Query(predicate, filterByDataAuth).FirstOrDefault();
        }

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>不跟踪数据更改（NoTracking）的查询数据源
        /// </summary>
        /// <returns>符合条件的数据集</returns>
        public virtual IQueryable<TEntity> QueryAsNoTracking()
        {
            return QueryAsNoTracking(null, true);
        }

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>不跟踪数据更改（NoTracking）的查询数据源
        /// </summary>
        /// <param name="predicate">数据查询谓语表达式</param>
        /// <returns>符合条件的数据集</returns>
        public virtual IQueryable<TEntity> QueryAsNoTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return QueryAsNoTracking(predicate, true);
        }

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>不跟踪数据更改（NoTracking）的查询数据源，并可附加过滤条件及是否启用数据权限过滤
        /// </summary>
        /// <param name="predicate">数据过滤表达式</param>
        /// <param name="filterByDataAuth">是否使用数据权限过滤，数据权限一般用于存在用户实例的查询，系统查询不启用数据权限过滤</param>
        /// <returns>符合条件的数据集</returns>
        public virtual IQueryable<TEntity> QueryAsNoTracking(Expression<Func<TEntity, bool>> predicate, bool filterByDataAuth)
        {
            return Query(predicate, filterByDataAuth).AsNoTracking();
        }

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>不跟踪数据更改（NoTracking）的查询数据源，并可Include导航属性
        /// </summary>
        /// <param name="includePropertySelectors">要Include操作的属性表达式</param>
        /// <returns>符合条件的数据集</returns>
        public virtual IQueryable<TEntity> QueryAsNoTracking(params Expression<Func<TEntity, object>>[] includePropertySelectors)
        {
            return Query(includePropertySelectors).AsNoTracking();
        }

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>跟踪数据更改（Tracking）的查询数据源
        /// </summary>
        /// <returns>符合条件的数据集</returns>
        public virtual IQueryable<TEntity> Query()
        {
            return Query(null, true);
        }

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>跟踪数据更改（Tracking）的查询数据源
        /// </summary>
        /// <param name="predicate">数据过滤表达式</param>
        /// <returns>符合条件的数据集</returns>
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return Query(predicate, true);
        }

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>跟踪数据更改（Tracking）的查询数据源，并可附加过滤条件及是否启用数据权限过滤
        /// </summary>
        /// <param name="predicate">数据过滤表达式</param>
        /// <param name="filterByDataAuth">是否使用数据权限过滤，数据权限一般用于存在用户实例的查询，系统查询不启用数据权限过滤</param>
        /// <returns>符合条件的数据集</returns>
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, bool filterByDataAuth)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            if (filterByDataAuth)
            {
                Expression<Func<TEntity, bool>> dataAuthExp = GetDataFilter(DataAuthOperation.Read);
                query = query.Where(dataAuthExp);
            }
            if (predicate == null)
            {
                return query;
            }
            return query.Where(predicate);
        }

        /// <summary>
        /// 获取<typeparamref name="TEntity"/>跟踪数据更改（Tracking）的查询数据源，并可Include导航属性
        /// </summary>
        /// <param name="includePropertySelectors">要Include操作的属性表达式</param>
        /// <returns>符合条件的数据集</returns>
        public virtual IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includePropertySelectors)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            if (includePropertySelectors == null || includePropertySelectors.Length == 0)
            {
                return query;
            }

            foreach (Expression<Func<TEntity, object>> selector in includePropertySelectors)
            {
                query = query.Include(selector);
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
        public virtual async Task<int> InsertAsync(params TEntity[] entities)
        {
            Check.NotNull(entities, nameof(entities));

            entities = CheckInsert(entities);
            await _dbSet.AddRangeAsync(entities, _cancellationTokenProvider.Token);
            return await _dbContext.SaveChangesAsync(_cancellationTokenProvider.Token);
        }

        /// <summary>
        /// 插入或更新实体
        /// </summary>
        /// <param name="entities">要处理的实体</param>
        /// <param name="existingFunc">实体是否存在的判断委托</param>
        /// <returns>操作影响的行数</returns>
        public virtual async Task<int> InsertOrUpdateAsync(TEntity[] entities, Func<TEntity, Expression<Func<TEntity, bool>>> existingFunc = null)
        {
            Check.NotNull(entities, nameof(entities));
            foreach (TEntity entity in entities)
            {
                Expression<Func<TEntity, bool>> exp = existingFunc == null
                    ? m => m.Id.Equals(entity.Id)
                    : existingFunc(entity);
                if (!await _dbSet.AnyAsync(exp))
                {
                    CheckInsert(entity);
                    await _dbSet.AddAsync(entity);
                }
                else
                {
                    CheckUpdate(entity);
                    ((DbContext)_dbContext).Update<TEntity, TKey>(entity);
                }
            }

            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 异步以DTO为载体批量插入实体
        /// </summary>
        /// <typeparam name="TInputDto">添加DTO类型</typeparam>
        /// <param name="dtos">添加DTO信息集合</param>
        /// <param name="checkAction">添加信息合法性检查委托</param>
        /// <param name="updateFunc">由DTO到实体的转换委托</param>
        /// <returns>业务操作结果</returns>
        public virtual async Task<OperationResult> InsertAsync<TInputDto>(ICollection<TInputDto> dtos,
            Func<TInputDto, Task> checkAction = null,
            Func<TInputDto, TEntity, Task<TEntity>> updateFunc = null) where TInputDto : IInputDto<TKey>
        {
            Check.NotNull(dtos, nameof(dtos));
            List<string> names = new List<string>();
            foreach (TInputDto dto in dtos)
            {
                try
                {
                    if (checkAction != null)
                    {
                        await checkAction(dto);
                    }
                    TEntity entity = dto.MapTo<TEntity>();
                    if (updateFunc != null)
                    {
                        entity = await updateFunc(dto, entity);
                    }
                    entity = CheckInsert(entity)[0];
                    await _dbSet.AddAsync(entity, _cancellationTokenProvider.Token);
                }
                catch (OsharpException e)
                {
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                names.AddIfNotNull(GetNameValue(dto));
            }
            int count = await _dbContext.SaveChangesAsync(_cancellationTokenProvider.Token);
            return count > 0
                ? new OperationResult(OperationResultType.Success,
                    names.Count > 0
                        ? $"信息 {names.ExpandAndToString()} 添加成功"
                        : $"{dtos.Count}个信息添加成功")
                : OperationResult.NoChanged;
        }

        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public virtual async Task<int> DeleteAsync(params TEntity[] entities)
        {
            Check.NotNull(entities, nameof(entities));

            DeleteInternal(entities);
            return await _dbContext.SaveChangesAsync(_cancellationTokenProvider.Token);
        }

        /// <summary>
        /// 异步删除指定编号的实体
        /// </summary>
        /// <param name="key">实体编号</param>
        /// <returns>操作影响的行数</returns>
        public virtual async Task<int> DeleteAsync(TKey key)
        {
            CheckEntityKey(key, nameof(key));

            TEntity entity = await _dbSet.FindAsync(key);
            return await DeleteAsync(entity);
        }

        /// <summary>
        /// 异步以标识集合批量删除实体
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="checkAction">删除前置检查委托</param>
        /// <param name="deleteFunc">删除委托，用于删除关联信息</param>
        /// <returns>业务操作结果</returns>
        public virtual async Task<OperationResult> DeleteAsync(ICollection<TKey> ids,
            Func<TEntity, Task> checkAction = null,
            Func<TEntity, Task<TEntity>> deleteFunc = null)
        {
            Check.NotNull(ids, nameof(ids));
            List<string> names = new List<string>();
            foreach (TKey id in ids)
            {
                TEntity entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    continue;
                }
                try
                {
                    if (checkAction != null)
                    {
                        await checkAction(entity);
                    }
                    if (deleteFunc != null)
                    {
                        entity = await deleteFunc(entity);
                    }
                    DeleteInternal(entity);
                }
                catch (OsharpException e)
                {
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                names.AddIfNotNull(GetNameValue(entity));
            }
            int count = await _dbContext.SaveChangesAsync(_cancellationTokenProvider.Token);
            return count > 0
                ? new OperationResult(OperationResultType.Success,
                    names.Count > 0
                        ? $"信息 {names.ExpandAndToString()} 删除成功"
                        : $"{ids.Count}个信息删除成功")
                : new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 异步删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        public virtual async Task<int> DeleteBatchAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Check.NotNull(predicate, nameof(predicate));
            // todo: 检测删除的数据权限

#if NET5_0
            await ((DbContextBase)_dbContext).BeginOrUseTransactionAsync(_cancellationTokenProvider.Token);
#else
            ((DbContextBase)_dbContext).BeginOrUseTransaction();
#endif
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                // 逻辑删除
                TEntity[] entities = _dbSet.Where(predicate).ToArray();
                DeleteInternal(entities);
                return await _dbContext.SaveChangesAsync(_cancellationTokenProvider.Token);
            }

            // 物理删除
            return await _dbSet.Where(predicate).DeleteAsync(_cancellationTokenProvider.Token);
        }

        /// <summary>
        /// 异步更新实体对象
        /// </summary>
        /// <param name="entities">更新后的实体对象</param>
        /// <returns>操作影响的行数</returns>
        public virtual async Task<int> UpdateAsync(params TEntity[] entities)
        {
            Check.NotNull(entities, nameof(entities));

            entities = CheckUpdate(entities);
            ((DbContext)_dbContext).Update<TEntity, TKey>(entities);
            return await _dbContext.SaveChangesAsync(_cancellationTokenProvider.Token);
        }

        /// <summary>
        /// 异步以DTO为载体批量更新实体
        /// </summary>
        /// <typeparam name="TEditDto">更新DTO类型</typeparam>
        /// <param name="dtos">更新DTO信息集合</param>
        /// <param name="checkAction">更新信息合法性检查委托</param>
        /// <param name="updateFunc">由DTO到实体的转换委托</param>
        /// <returns>业务操作结果</returns>
        public virtual async Task<OperationResult> UpdateAsync<TEditDto>(ICollection<TEditDto> dtos,
            Func<TEditDto, TEntity, Task> checkAction = null,
            Func<TEditDto, TEntity, Task<TEntity>> updateFunc = null) where TEditDto : IInputDto<TKey>
        {
            List<string> names = new List<string>();
            foreach (TEditDto dto in dtos)
            {
                try
                {
                    TEntity entity = await _dbSet.FindAsync(dto.Id);
                    if (entity == null)
                    {
                        return new OperationResult(OperationResultType.QueryNull);
                    }
                    if (checkAction != null)
                    {
                        await checkAction(dto, entity);
                    }
                    entity = dto.MapTo(entity);
                    if (updateFunc != null)
                    {
                        entity = await updateFunc(dto, entity);
                    }
                    entity = CheckUpdate(entity)[0];
                    ((DbContext)_dbContext).Update<TEntity, TKey>(entity);
                }
                catch (OsharpException e)
                {
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    return new OperationResult(OperationResultType.Error, e.Message);
                }
                names.AddIfNotNull(GetNameValue(dto));
            }
            int count = await _dbContext.SaveChangesAsync(_cancellationTokenProvider.Token);
            return count > 0
                ? new OperationResult(OperationResultType.Success,
                    names.Count > 0
                        ? $"信息 {names.ExpandAndToString()} 更新成功"
                        : $"{dtos.Count}个信息更新成功")
                : new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 异步更新所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="updateExpression">实体更新表达式</param>
        /// <returns>操作影响的行数</returns>
        public virtual async Task<int> UpdateBatchAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Check.NotNull(predicate, nameof(predicate));
            Check.NotNull(updateExpression, nameof(updateExpression));

#if NET5_0
            await ((DbContextBase)_dbContext).BeginOrUseTransactionAsync(_cancellationTokenProvider.Token);
#else
            ((DbContextBase)_dbContext).BeginOrUseTransaction();
#endif
            return await _dbSet.Where(predicate).UpdateAsync(updateExpression, _cancellationTokenProvider.Token);
        }

        /// <summary>
        /// 异步检查实体是否存在
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="id">编辑的实体标识</param>
        /// <returns>是否存在</returns>
        public virtual async Task<bool> CheckExistsAsync(Expression<Func<TEntity, bool>> predicate, TKey id = default)
        {
            predicate.CheckNotNull(nameof(predicate));

            TKey defaultId = default;
            var entity = await _dbSet.Where(predicate).Select(m => new { m.Id }).FirstOrDefaultAsync(_cancellationTokenProvider.Token);
            bool exists = !typeof(TKey).IsValueType && ReferenceEquals(id, null) || id.Equals(defaultId)
                ? entity != null
                : entity != null && !EntityBase<TKey>.IsKeyEqual(entity.Id, id);
            return exists;
        }

        /// <summary>
        /// 异步查找指定主键的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>符合主键的实体，不存在时返回null</returns>
        public virtual async Task<TEntity> GetAsync(TKey key)
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

        private void SetEmptyGuidKey(TEntity entity)
        {
            Type keyType = typeof(TKey);
            //自增int
            if (keyType == typeof(int))
            {
                IKeyGenerator<int> generator = _serviceProvider.GetService<IKeyGenerator<int>>();
                entity.Id = generator.Create().CastTo<TKey>();
                return;
            }
            //雪花long
            if (keyType == typeof(long))
            {
                IKeyGenerator<long> generator = _serviceProvider.GetService<IKeyGenerator<long>>();
                entity.Id = generator.Create().CastTo<TKey>();
            }
            //顺序guid
            if (keyType == typeof(Guid) && entity.Id.Equals(Guid.Empty))
            {
                DatabaseType databaseType = _dbContext.GetDatabaseType();
                ISequentialGuidGenerator generator =
                    _serviceProvider.GetServices<ISequentialGuidGenerator>().FirstOrDefault(m => m.DatabaseType == databaseType);
                entity.Id = generator == null ? SequentialGuid.Create(databaseType).CastTo<TKey>() : generator.Create().CastTo<TKey>();
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

        /// <summary>
        /// 检查指定操作的数据权限，验证要操作的数据是否符合特定的验证委托
        /// </summary>
        /// <param name="operation">数据权限操作</param>
        /// <param name="entities">要验证的实体对象</param>
        private static void CheckDataAuth(DataAuthOperation operation, params TEntity[] entities)
        {
            if (entities.Length == 0)
            {
                return;
            }
            Expression<Func<TEntity, bool>> exp = GetDataFilter(operation);
            Func<TEntity, bool> func = exp.Compile();
            bool flag = entities.All(func);
            if (!flag)
            {
                throw new OsharpException($"实体 {typeof(TEntity)} 的数据 {entities.ExpandAndToString(m => m.Id.ToString())} 进行 {operation.ToDescription()} 操作时权限不足");
            }
        }

        private static Expression<Func<TEntity, bool>> GetDataFilter(DataAuthOperation operation)
        {
            return FilterHelper.GetDataFilterExpression<TEntity>(operation: operation);
        }

        private TEntity[] CheckInsert(params TEntity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                TEntity entity = entities[i];
                SetEmptyGuidKey(entity);
                entities[i] = entity.CheckICreatedTime<TEntity, TKey>();

                string userIdTypeName = _principal?.Identity.GetClaimValueFirstOrDefault(OsharpConstants.UserIdTypeName);
                if (userIdTypeName == null)
                {
                    continue;
                }
                entity = entities[i];
                if (userIdTypeName == typeof(int).FullName)
                {
                    entities[i] = entity.CheckICreationAudited<TEntity, TKey, int>(_principal);
                }
                else if (userIdTypeName == typeof(Guid).FullName)
                {
                    entities[i] = entity.CheckICreationAudited<TEntity, TKey, Guid>(_principal);
                }
                else
                {
                    entities[i] = entity.CheckICreationAudited<TEntity, TKey, long>(_principal);
                }
            }

            return entities;
        }

        private TEntity[] CheckUpdate(params TEntity[] entities)
        {
            CheckDataAuth(DataAuthOperation.Update, entities);

            string userIdTypeName = _principal?.Identity.GetClaimValueFirstOrDefault(OsharpConstants.UserIdTypeName);
            if (userIdTypeName == null)
            {
                return entities;
            }
            for (var i = 0; i < entities.Length; i++)
            {
                TEntity entity = entities[i];
                if (userIdTypeName == typeof(int).FullName)
                {
                    entities[i] = entity.CheckIUpdateAudited<TEntity, TKey, int>(_principal);
                }
                else if (userIdTypeName == typeof(Guid).FullName)
                {
                    entities[i] = entity.CheckIUpdateAudited<TEntity, TKey, Guid>(_principal);
                }
                else
                {
                    entities[i] = entity.CheckIUpdateAudited<TEntity, TKey, long>(_principal);
                }
            }

            return entities;
        }

        private void DeleteInternal(params TEntity[] entities)
        {
            CheckDataAuth(DataAuthOperation.Delete, entities);
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                // 逻辑删除
                foreach (TEntity entity in entities)
                {
                    ISoftDeletable softDeletableEntity = (ISoftDeletable)entity;
                    softDeletableEntity.DeletedTime = DateTime.Now;
                }
            }
            else
            {
                // 物理删除
                _dbSet.RemoveRange(entities);
            }
        }

        #endregion
    }
}