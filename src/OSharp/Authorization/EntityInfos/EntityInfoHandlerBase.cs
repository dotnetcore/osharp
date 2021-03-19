// -----------------------------------------------------------------------
//  <copyright file="EntityInfoHandlerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:14</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Collections;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Reflection;


namespace OSharp.Authorization.EntityInfos
{
    /// <summary>
    /// 实体信息处理基类
    /// </summary>
    /// <typeparam name="TEntityInfo"></typeparam>
    /// <typeparam name="TEntityInfoHandler"></typeparam>
    public abstract class EntityInfoHandlerBase<TEntityInfo, TEntityInfoHandler> : IEntityInfoHandler
        where TEntityInfo : class, IEntityInfo, new()
    {
        private readonly List<TEntityInfo> _entityInfos = new List<TEntityInfo>();
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="EntityInfoHandlerBase{TEntityInfo,TEntityInfoProvider}"/>类型的新实例
        /// </summary>
        protected EntityInfoHandlerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetLogger<TEntityInfoHandler>();
        }

        /// <summary>
        /// 从程序集中刷新实体信息（实现了<see cref="IEntity{TKey}"/>接口的实体类）
        /// </summary>
        public void Initialize()
        {
            Type[] entityTypes = GetEntityTypes();
            _logger.LogInformation($"数据实体处理器开始初始化，共找到 {entityTypes.Length} 个实体类");
            foreach (Type entityType in entityTypes)
            {
                if (_entityInfos.Exists(m => m.TypeName == entityType.FullName))
                {
                    continue;
                }

                TEntityInfo entityInfo = new TEntityInfo();
                entityInfo.FromType(entityType);
                _entityInfos.Add(entityInfo);
                _logger.LogDebug($"提取实体信息：{entityInfo}]");
            }

            _serviceProvider.ExecuteScopedWork(provider =>
            {
                SyncToDatabase(provider, _entityInfos);
            });

            RefreshCache();
        }

        protected virtual Type[] GetEntityTypes()
        {
            return AssemblyManager.FindTypesByBase(typeof(IEntity<>));
        }

        /// <summary>
        /// 查找指定实体类型的实体信息
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns>实体信息</returns>
        public IEntityInfo GetEntityInfo(Type type)
        {
            Check.NotNull(type, nameof(type));
            if (_entityInfos.Count == 0)
            {
                RefreshCache();
            }

            string typeName = type.GetFullNameWithModule();
            IEntityInfo entityInfo = _entityInfos.FirstOrDefault(m => m.TypeName == typeName);
            if (entityInfo != null)
            {
                return entityInfo;
            }

            if (type.BaseType == null)
            {
                return null;
            }

            typeName = type.BaseType.GetFullNameWithModule();
            return _entityInfos.FirstOrDefault(m => m.TypeName == typeName);
        }

        /// <summary>
        /// 查找指定实体类型的实体信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns>实体信息</returns>
        public IEntityInfo GetEntityInfo<TEntity, TKey>() where TEntity : IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Type type = typeof(TEntity);
            return GetEntityInfo(type);
        }

        /// <summary>
        /// 刷新实体信息缓存
        /// </summary>
        public void RefreshCache()
        {
            _serviceProvider.ExecuteScopedWork(provider =>
            {
                _entityInfos.Clear();
                _entityInfos.AddRange(GetFromDatabase(provider));
                _logger.LogInformation($"刷新实体信息缓存，从数据库获取到 {_entityInfos.Count} 个实体信息");
            });
        }

        /// <summary>
        /// 将从程序集获取的实体信息同步到数据库
        /// </summary>
        protected virtual void SyncToDatabase(IServiceProvider scopedProvider, List<TEntityInfo> entityInfos)
        {
            IRepository<TEntityInfo, Guid> repository = scopedProvider.GetService<IRepository<TEntityInfo, Guid>>();
            if (repository == null)
            {
                _logger.LogWarning("初始化实体数据时，IRepository<,>的服务未找到，请初始化 EntityFrameworkCorePack 模块");
                return;
            }

            IUnitOfWork unitOfWork = scopedProvider.GetUnitOfWork(true);

            //检查指定实体的Hash值，决定是否需要进行数据库同步
            if (!entityInfos.CheckSyncByHash(scopedProvider, _logger))
            {
                _logger.LogInformation("同步实体数据时，数据签名与上次相同，取消同步");
                return;
            }

            TEntityInfo[] dbItems = repository.Query(null, false).ToArray();

            //删除的实体信息
            TEntityInfo[] removeItems = dbItems.Except(entityInfos, EqualityHelper<TEntityInfo>.CreateComparer(m => m.TypeName)).ToArray();
            int removeCount = removeItems.Length;
            //todo：由于外键关联不能物理删除的实体，需要实现逻辑删除
            foreach (TEntityInfo entityInfo in removeItems)
            {
                repository.Delete(entityInfo);
                _logger.LogDebug($"删除实体信息：{entityInfo.Name}[{entityInfo.TypeName}]");
            }

            //处理新增的实体信息
            TEntityInfo[] addItems = entityInfos.Except(dbItems, EqualityHelper<TEntityInfo>.CreateComparer(m => m.TypeName)).ToArray();
            int addCount = addItems.Length;
            foreach (TEntityInfo entityInfo in addItems)
            {
                repository.Insert(entityInfo);
                _logger.LogDebug($"新增实体信息：{entityInfo.Name}[{entityInfo.TypeName}]");
            }

            //处理更新的实体信息
            int updateCount = 0;
            foreach (TEntityInfo item in dbItems.Except(removeItems))
            {
                bool isUpdate = false;
                TEntityInfo entityInfo = entityInfos.SingleOrDefault(m => m.TypeName == item.TypeName);
                if (entityInfo == null)
                {
                    continue;
                }

                if (item.Name != entityInfo.Name)
                {
                    item.Name = entityInfo.Name;
                    isUpdate = true;
                }

                if (item.PropertyJson != entityInfo.PropertyJson)
                {
                    item.PropertyJson = entityInfo.PropertyJson;
                    isUpdate = true;
                }

                if (isUpdate)
                {
                    repository.Update(item);
                    _logger.LogDebug($"更新实体信息：{entityInfo.Name}[{entityInfo.TypeName}]");
                    updateCount++;
                }
            }

            unitOfWork.Commit();
            if (removeCount + addCount + updateCount > 0)
            {
                string msg = "刷新实体信息";
                if (addCount > 0)
                {
                    msg += $"，添加实体信息 {addCount} 个";
                }

                if (updateCount > 0)
                {
                    msg += $"，更新实体信息 {updateCount} 个";
                }

                if (removeCount > 0)
                {
                    msg += $"，删除实体信息 {removeCount} 个";
                }

                _logger.LogInformation(msg);
            }
        }

        /// <summary>
        /// 从数据库获取最新实体信息
        /// </summary>
        /// <returns></returns>
        protected virtual TEntityInfo[] GetFromDatabase(IServiceProvider scopedProvider)
        {
            IRepository<TEntityInfo, Guid> repository = scopedProvider.GetService<IRepository<TEntityInfo, Guid>>();
            if (repository == null)
            {
                return new TEntityInfo[0];
            }

            TEntityInfo[] entityInfos = repository.QueryAsNoTracking(null, false).ToArray();
            return entityInfos;
        }
    }
}