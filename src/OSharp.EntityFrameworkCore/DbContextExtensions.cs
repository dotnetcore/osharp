// -----------------------------------------------------------------------
//  <copyright file="DbContextExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-20 1:06</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

using OSharp.Audits;
using OSharp.Collections;
using OSharp.Core.EntityInfos;
using OSharp.Core.Functions;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Exceptions;


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
        /// 检测关系型数据库是否存在
        /// </summary>
        public static bool ExistsRelationalDatabase(this DbContext context)
        {
            return context.Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator creator && creator.Exists();
        }

        /// <summary>
        /// 获取未提交的迁移记录并提交迁移
        /// </summary>
        public static void CheckAndMigration(this DbContext dbContext)
        {
            string[] migrations = dbContext.Database.GetPendingMigrations().ToArray();
            if (migrations.Length > 0)
            {
                dbContext.Database.Migrate();
                ILogger logger = ServiceLocator.Instance.GetLogger("OSharp.Entity.DbContextExtensions");
                logger.LogInformation($"已提交{migrations.Length}条挂起的迁移记录：{migrations.ExpandAndToString()}");
            }
        }

        /// <summary>
        /// 执行指定的Sql语句
        /// </summary>
        public static int ExecuteSqlCommand(this IDbContext dbContext, string sql, params object[] parameters)
        {
            if (!(dbContext is DbContext context))
            {
                throw new OsharpException($"参数dbContext类型为“{dbContext.GetType()}”，不能转换为 DbContext");
            }
            return context.Database.ExecuteSqlCommand(new RawSqlString(sql), parameters);
        }

        /// <summary>
        /// 异步执行指定的Sql语句
        /// </summary>
        public static Task<int> ExecuteSqlCommandAsync(this IDbContext dbContext, string sql, params object[] parameters)
        {
            if (!(dbContext is DbContext context))
            {
                throw new OsharpException($"参数dbContext类型为“{dbContext.GetType()}”，不能转换为 DbContext");
            }
            return context.Database.ExecuteSqlCommandAsync(new RawSqlString(sql), parameters);
        }

        /// <summary>
        /// 更新上下文中指定实体的状态
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">主键类型</typeparam>
        /// <param name="context">上下文对象</param>
        /// <param name="entities">要更新的实体类型</param>
        public static void Update<TEntity, TKey>(this DbContext context, params TEntity[] entities)
            where TEntity : class, IEntity<TKey>
        {
            Check.NotNull(entities, nameof(entities));

            DbSet<TEntity> set = context.Set<TEntity>();
            foreach (TEntity entity in entities)
            {
                try
                {
                    EntityEntry<TEntity> entry = context.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        set.Attach(entity);
                        entry.State = EntityState.Modified;
                    }
                }
                catch (InvalidOperationException)
                {
                    TEntity oldEntity = set.Find(entity.Id);
                    context.Entry(oldEntity).CurrentValues.SetValues(entity);
                }
            }
        }

        /// <summary>
        /// 获取上下文实体审计数据
        /// </summary>
        public static IList<AuditEntityEntry> GetAuditEntities(this DbContext context)
        {
            List<AuditEntityEntry> result = new List<AuditEntityEntry>();
            //当前操作的功能是否允许数据审计
            ScopedDictionary scopedDict = ServiceLocator.Instance.GetService<ScopedDictionary>();
            IFunction function = scopedDict?.Function;
            if (function == null || !function.AuditEntityEnabled)
            {
                return result;
            }
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
                //当前操作的实体是否允许数据审计
                IEntityInfo entityInfo = entityInfoHandler.GetEntityInfo(entry.Entity.GetType());
                if (entityInfo == null || !entityInfo.AuditEnabled)
                {
                    continue;
                }
                result.AddIfNotNull(GetAuditEntity(entry, entityInfo));
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
                    DisplayName = entityProperties.First(m => m.Name == name).Display,
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