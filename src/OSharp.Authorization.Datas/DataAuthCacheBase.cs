// -----------------------------------------------------------------------
//  <copyright file="DataAuthCacheBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-04 17:33</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Authorization.Entities;
using OSharp.Authorization.EntityInfos;
using OSharp.Caching;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Filter;
using OSharp.Identity.Entities;


namespace OSharp.Authorization
{
    /// <summary>
    /// 数据权限缓存基类
    /// </summary>
    public abstract class DataAuthCacheBase<TEntityRole, TRole, TEntityInfo, TRoleKey> : IDataAuthCache
        where TEntityRole : EntityRoleBase<TRoleKey>
        where TRole : RoleBase<TRoleKey>
        where TEntityInfo : class, IEntityInfo
        where TRoleKey : IEquatable<TRoleKey>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="DataAuthCacheBase{TEntityRole, TRole, TEntityInfo, TRoleKey}"/>类型的新实例
        /// </summary>
        protected DataAuthCacheBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cache = serviceProvider.GetService<IDistributedCache>();
            _logger = serviceProvider.GetLogger(GetType());
        }

        /// <summary>
        /// 创建数据权限缓存
        /// </summary>
        public virtual void BuildCaches()
        {
            var entityRoles = _serviceProvider.ExecuteScopedWork(provider =>
            {
                IRepository<TEntityRole, Guid> entityRoleRepository = provider.GetService<IRepository<TEntityRole, Guid>>();
                IRepository<TRole, TRoleKey> roleRepository = provider.GetService<IRepository<TRole, TRoleKey>>();
                IRepository<TEntityInfo, Guid> entityInfoRepository = provider.GetService<IRepository<TEntityInfo, Guid>>();
                return entityRoleRepository.QueryAsNoTracking(m => !m.IsLocked).Select(m => new
                {
                    m.FilterGroupJson,
                    m.Operation,
                    RoleName = roleRepository.QueryAsNoTracking(null, false).Where(n => n.Id.Equals(m.RoleId)).Select(n => n.Name).FirstOrDefault(),
                    EntityTypeFullName = entityInfoRepository.QueryAsNoTracking(null, false).Where(n => n.Id == m.EntityId).Select(n => n.TypeName).FirstOrDefault()
                }).ToArray();
            });

            foreach (var entityRole in entityRoles)
            {
                FilterGroup filterGroup = entityRole.FilterGroupJson.FromJsonString<FilterGroup>();
                string key = GetKey(entityRole.RoleName, entityRole.EntityTypeFullName, entityRole.Operation);
                string name = GetName(entityRole.RoleName, entityRole.EntityTypeFullName, entityRole.Operation);

                _cache.Set(key, filterGroup);
                _logger.LogDebug($"创建{name}的数据权限规则缓存");
            }
            _logger.LogInformation($"数据权限：创建{entityRoles.Length}个数据权限过滤规则缓存");
        }

        /// <summary>
        /// 设置指定数据权限的缓存
        /// </summary>
        /// <param name="item">数据权限缓存项</param>
        public virtual void SetCache(DataAuthCacheItem item)
        {
            string key = GetKey(item.RoleName, item.EntityTypeFullName, item.Operation);
            string name = GetName(item.RoleName, item.EntityTypeFullName, item.Operation);

            _cache.Set(key, item.FilterGroup);
            _logger.LogDebug($"创建{name}的数据权限规则缓存");
        }

        /// <summary>
        /// 移除指定角色名与实体类型的缓存项
        /// </summary>
        /// <param name="item">要移除的数据权限缓存项信息</param>
        public virtual void RemoveCache(DataAuthCacheItem item)
        {
            string key = GetKey(item.RoleName, item.EntityTypeFullName, item.Operation);
            string name = GetName(item.RoleName, item.EntityTypeFullName, item.Operation);
            _cache.Remove(key);
            _logger.LogDebug($"移除{name}的数据权限规则缓存");
        }

        /// <summary>
        /// 获取指定角色名与实体类型的数据权限过滤规则
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="entityTypeFullName">实体类型名称</param>
        /// <returns>数据过滤条件组</returns>
        /// <param name="operation">数据权限操作</param>
        public virtual FilterGroup GetFilterGroup(string roleName, string entityTypeFullName, DataAuthOperation operation)
        {
            string key = GetKey(roleName, entityTypeFullName, operation);
            return _cache.Get<FilterGroup>(key);
        }

        private static string GetKey(string roleName, string entityTypeFullName, DataAuthOperation operation)
        {
            return $"Auth:Data:EntityRole:{roleName}:{entityTypeFullName}:{operation}";
        }

        private static string GetName(string roleName, string entityTypeFullName, DataAuthOperation operation)
        {
            return $"角色“{roleName}”和实体“{entityTypeFullName}”和操作“{operation}”";
        }
    }
}