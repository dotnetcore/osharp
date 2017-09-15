using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Collections;
using OSharp.Entity;


namespace OSharp.Infrastructure
{
    /// <summary>
    /// 实体信息处理基类
    /// </summary>
    /// <typeparam name="TEntityInfo"></typeparam>
    /// <typeparam name="TEntityInfoHandler"></typeparam>
    public abstract class EntityInfoHandlerBase<TEntityInfo, TEntityInfoHandler> : IEntityInfoHandler, IDisposable
        where TEntityInfo : class, IEntityInfo, IEntity<Guid>, new()
    {
        private readonly IServiceScope _scope;
        private readonly IServiceProvider _scopedServiceProvider;
        private readonly ILogger _logger;
        private readonly List<TEntityInfo> _entityInfos = new List<TEntityInfo>();

        /// <summary>
        /// 初始化一个<see cref="EntityInfoHandlerBase{TEntityInfo,TEntityInfoProvider}"/>类型的新实例
        /// </summary>
        protected EntityInfoHandlerBase(IServiceProvider applicationServiceProvider)
        {
            _scope = applicationServiceProvider.CreateScope();
            _scopedServiceProvider = _scope.ServiceProvider;
            _logger = _scopedServiceProvider.GetService<ILogger<TEntityInfoHandler>>();
        }

        /// <summary>
        /// 从程序集中刷新实体信息（实现了<see cref="IEntity{TKey}"/>接口的实体类）
        /// </summary>
        public void Initialize()
        {
            IEntityTypeFinder entityTypeFinder = _scopedServiceProvider.GetService<IEntityTypeFinder>();
            Type[] entityTypes = entityTypeFinder.FindAll(true);

            foreach (Type entityType in entityTypes)
            {
                if (_entityInfos.Exists(m => m.ClassFullName == entityType.FullName))
                {
                    continue;
                }
                TEntityInfo entityInfo = new TEntityInfo();
                entityInfo.FromType(entityType);
                _entityInfos.Add(entityInfo);
            }

            SyncToDatabase(_entityInfos);
            RefreshCache();
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
            return _entityInfos.FirstOrDefault(m => m.ClassFullName == type.FullName)
                ?? _entityInfos.FirstOrDefault(m => type.BaseType != null && m.ClassFullName == type.BaseType.FullName);
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
            _entityInfos.Clear();
            _entityInfos.AddRange(GetFromDatabase());
        }

        /// <summary>
        /// 将从程序集获取的实体信息同步到数据库
        /// </summary>
        protected virtual void SyncToDatabase(List<TEntityInfo> entityInfos)
        {
            IRepository<TEntityInfo, Guid> repository = _scopedServiceProvider.GetService<IRepository<TEntityInfo, Guid>>();
            TEntityInfo[] dbItems = repository.TrackQuery().ToArray();

            //删除的实体信息
            TEntityInfo[] removeItems = dbItems.Except(entityInfos, EqualityHelper<TEntityInfo>.CreateComparer(m => m.ClassFullName)).ToArray();
            int removeCount = removeItems.Length;
            //todo：由于外键关联不能物理删除的实体，需要实现逻辑删除
            repository.Delete(removeItems);

            //处理新增的实体信息
            TEntityInfo[] addItems = entityInfos.Except(dbItems, EqualityHelper<TEntityInfo>.CreateComparer(m => m.ClassFullName)).ToArray();
            int addCount = addItems.Length;
            repository.Insert(addItems);

            //处理更新的实体信息
            int updateCount = 0;
            foreach (TEntityInfo item in dbItems.Except(removeItems))
            {
                bool isUpdate = false;
                TEntityInfo entityInfo = entityInfos.SingleOrDefault(m => m.ClassFullName == item.ClassFullName);
                if (entityInfo == null)
                {
                    continue;
                }
                if (item.Name != entityInfo.Name)
                {
                    item.Name = entityInfo.Name;
                    isUpdate = true;
                }
                if (item.PropertyNamesJson != entityInfo.PropertyNamesJson)
                {
                    item.PropertyNamesJson = entityInfo.PropertyNamesJson;
                    isUpdate = true;
                }
                if (isUpdate)
                {
                    repository.Update(item);
                    updateCount++;
                }
            }
            repository.UnitOfWork.Commit();
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
        protected virtual TEntityInfo[] GetFromDatabase()
        {
            IRepository<TEntityInfo, Guid> repository = _scopedServiceProvider.GetService<IRepository<TEntityInfo, Guid>>();
            return repository.Query().ToArray();
        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _scope?.Dispose();
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
