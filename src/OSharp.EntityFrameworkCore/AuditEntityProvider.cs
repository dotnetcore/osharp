// -----------------------------------------------------------------------
//  <copyright file="AuditEntityProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-08-22 10:02</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Audits;
using OSharp.Authorization.EntityInfos;
using OSharp.Authorization.Functions;
using OSharp.Collections;
using OSharp.Dependency;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 数据审计信息提供者
    /// </summary>
    public class AuditEntityProvider : IAuditEntityProvider
    {
        private readonly IEntityInfoHandler _entityInfoHandler;
        private readonly ILogger _logger;
        private readonly ScopedDictionary _scopedDict;

        /// <summary>
        /// 初始化一个<see cref="AuditEntityProvider"/>类型的新实例
        /// </summary>
        public AuditEntityProvider(IServiceProvider provider)
        {
            _logger = provider.GetLogger(this);
            _scopedDict = provider.GetService<ScopedDictionary>();
            _entityInfoHandler = provider.GetService<IEntityInfoHandler>();
        }

        /// <summary>
        /// 从指定上下文中获取数据审计信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <returns>数据审计信息的集合</returns>
        public virtual IEnumerable<AuditEntityEntry> GetAuditEntities(DbContext context)
        {
            List<AuditEntityEntry> result = new List<AuditEntityEntry>();
            //当前操作的功能是否允许数据审计
            IFunction function = _scopedDict?.Function;
            if (function == null)
            {
                return result;
            }

            if (!function.AuditEntityEnabled)
            {
                _logger.LogDebug($"提取数据审计：功能 {function} 未开启数据审计，取消操作");
                return result;
            }

            if (_entityInfoHandler == null)
            {
                return result;
            }

            EntityState[] states = { EntityState.Added, EntityState.Modified, EntityState.Deleted };
            List<EntityEntry> entries = context.ChangeTracker.Entries().Where(m => m.Entity != null && states.Contains(m.State)).ToList();
            if (entries.Count == 0)
            {
                return result;
            }

            foreach (EntityEntry entry in entries)
            {
                //当前操作的实体是否允许数据审计
                IEntityInfo entityInfo = _entityInfoHandler.GetEntityInfo(entry.Entity.GetType());
                if (entityInfo == null)
                {
                    continue;
                }

                if (!entityInfo.AuditEnabled)
                {
                    _logger.LogDebug($"提取数据审计：实体 {entityInfo} 未开启审计，跳过");
                    continue;
                }

                result.AddIfNotNull(GetAuditEntity(entry, entityInfo));
                _logger.LogDebug($"提取数据审计：实体 {entityInfo} ");
            }

            return result;
        }

        private static AuditEntityEntry GetAuditEntity(EntityEntry entry, IEntityInfo entityInfo)
        {
            AuditEntityEntry audit = new AuditEntityEntry
            {
                Name = entityInfo.Name,
                TypeName = entityInfo.TypeName,
                OperateType = entry.State == EntityState.Added
                    ? OperateType.Insert
                    : entry.State == EntityState.Modified
                        ? OperateType.Update
                        : entry.State == EntityState.Deleted
                            ? OperateType.Delete
                            : OperateType.Query,
                Entity = entry.Entity
            };
            EntityProperty[] entityProperties = entityInfo.Properties;
            foreach (IProperty property in entry.CurrentValues.Properties)
            {
                if (property.IsConcurrencyToken)
                {
                    continue;
                }

                if (property.PropertyInfo == null || property.PropertyInfo.HasAttribute<AuditIgnoreAttribute>())
                {
                    continue;
                }

                string name = property.Name;
                if (property.IsPrimaryKey())
                {
                    audit.EntityKey = entry.State == EntityState.Deleted
                        ? entry.Property(property.Name).OriginalValue?.ToString()
                        : entry.Property(property.Name).CurrentValue?.ToString();
                }

                AuditPropertyEntry auditProperty = new AuditPropertyEntry()
                {
                    FieldName = name,
                    DisplayName = entityProperties.FirstOrDefault(m => m.Name == name)?.Display ?? name,
                    DataType = property.ClrType.ToString()
                };
                if (entry.State == EntityState.Added)
                {
                    auditProperty.NewValue = entry.Property(property.Name).CurrentValue?.ToString();
                    audit.PropertyEntries.Add(auditProperty);
                }
                else if (entry.State == EntityState.Deleted)
                {
                    auditProperty.OriginalValue = entry.Property(property.Name).OriginalValue?.ToString();
                    audit.PropertyEntries.Add(auditProperty);
                }
                else if (entry.State == EntityState.Modified)
                {
                    string currentValue = entry.Property(property.Name).CurrentValue?.ToString();
                    string originalValue = entry.Property(property.Name).OriginalValue?.ToString();
                    if (currentValue == originalValue)
                    {
                        continue;
                    }

                    auditProperty.NewValue = currentValue;
                    auditProperty.OriginalValue = originalValue;
                    audit.PropertyEntries.Add(auditProperty);
                }
            }

            return audit.PropertyEntries.Count == 0 ? null : audit;
        }
    }
}