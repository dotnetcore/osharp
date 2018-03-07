// -----------------------------------------------------------------------
//  <copyright file="DbContextExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-20 1:06</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

using OSharp.Audits;
using OSharp.Core;


namespace OSharp.Entity
{
    /// <summary>
    /// 数据上下文扩展方法
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// 当前上下文是否是关系型数据库
        /// </summary>
        public static bool IsRelationalTransaction(this DbContext context)
        {
            return context.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
        }

        /// <summary>
        /// 获取迁移记录并提交迁移
        /// </summary>
        public static void CheckAndMigration(this DbContext dbContext)
        {
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
        }

        /// <summary>
        /// 获取上下文实体审计数据
        /// </summary>
        public static IList<AuditEntity> GetAuditEntities(this DbContext context)
        {
            List<AuditEntity> result = new List<AuditEntity>();
            IEntityInfoHandler entityInfoHandler = ServiceLocator.Instance.GetService<IEntityInfoHandler>();
            if (entityInfoHandler == null)
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
                IEntityInfo entityInfo = entityInfoHandler.GetEntityInfo(entry.Entity.GetType());
                if (entityInfo == null || !entityInfo.AuditEnabled)
                {
                    continue;
                }
                result.Add(GetAuditEntity(entry, entityInfo));
            }
            return result;
        }

        private static AuditEntity GetAuditEntity(EntityEntry entry, IEntityInfo entityInfo)
        {
            AuditEntity audit = new AuditEntity() { Name = entityInfo.Name, TypeName = entityInfo.TypeName, OperateType = OperateType.Insert };
            foreach (IProperty property in entry.CurrentValues.Properties)
            {
                if (property.IsConcurrencyToken)
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
                AuditEntityProperty auditProperty = new AuditEntityProperty()
                {
                    Name = name,
                    FieldName = entityInfo.PropertyNames[name],
                    DataType = property.ClrType.ToString()
                };
                if (entry.State == EntityState.Added)
                {
                    auditProperty.NewValue = entry.Property(property.Name).CurrentValue?.ToString();
                }
                else if (entry.State == EntityState.Deleted)
                {
                    auditProperty.OriginalValue = entry.Property(property.Name).OriginalValue?.ToString();
                }
                else if (entry.State == EntityState.Modified)
                {
                    string currentValue = entry.Property(property.Name).CurrentValue?.ToString();
                    string originalValue = entry.Property(property.Name).OriginalValue?.ToString();
                    if (currentValue != originalValue)
                    {
                        auditProperty.NewValue = currentValue;
                        auditProperty.OriginalValue = originalValue;
                    }
                }
                audit.Properties.Add(auditProperty);
            }
            return audit;
        }
    }
}