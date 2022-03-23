// -----------------------------------------------------------------------
//  <copyright file="EntityManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2019-06-27 9:04</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Authorization.EntityInfos;
using OSharp.Authorization.Functions;
using OSharp.Collections;
using OSharp.Core.Data;
using OSharp.Core.Systems;
using OSharp.Exceptions;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体管理器
    /// </summary>
    public class EntityManager : IEntityManager
    {
        private readonly ConcurrentDictionary<Type, IEntityRegister[]> _entityRegistersDict
            = new ConcurrentDictionary<Type, IEntityRegister[]>();
        private readonly ILogger _logger;
        private bool _initialized;

        /// <summary>
        /// 初始化一个<see cref="EntityManager"/>类型的新实例
        /// </summary>
        public EntityManager(IServiceProvider provider)
        {
            _logger = provider.GetLogger<EntityManager>();
        }

        /// <summary>
        /// 初始化实体类型注册
        /// </summary>
        public virtual void Initialize()
        {
            var dict = _entityRegistersDict;
            Type[] types = AssemblyManager.FindTypesByBase<IEntityRegister>();
            if (types.Length == 0 || _initialized)
            {
                _logger.LogDebug("数据库上下文实体已初始化，跳过");
                return;
            }

            //创建实体映射类的实例
            List<IEntityRegister> registers = types.Select(type => Activator.CreateInstance(type) as IEntityRegister).ToList();
            List<IGrouping<Type, IEntityRegister>> groups = registers.GroupBy(m => m.DbContextType).ToList();
            Type key;
            dict.Clear();
            foreach (IGrouping<Type, IEntityRegister> group in groups)
            {
                key = group.Key ?? typeof(DefaultDbContext);
                List<IEntityRegister> list = dict.ContainsKey(key) ? dict[key].ToList() : new List<IEntityRegister>();
                list.AddRange(group);
                dict[key] = list.ToArray();
            }

            //添加框架的一些默认实体的实体映射信息（如果不存在）
            key = typeof(DefaultDbContext);
            if (dict.ContainsKey(key))
            {
                List<IEntityRegister> list = dict[key].ToList();
                list.AddIfNotExist(new EntityInfoConfiguration(), m => m.EntityType.IsBaseOn<IEntityInfo>());
                list.AddIfNotExist(new FunctionConfiguration(), m => m.EntityType.IsBaseOn<IFunction>());
                list.AddIfNotExist(new KeyValueConfiguration(), m => m.EntityType.IsBaseOn<IKeyValue>());
                dict[key] = list.ToArray();
            }

            foreach (KeyValuePair<Type, IEntityRegister[]> item in dict)
            {
                foreach (IEntityRegister register in item.Value)
                {
                    _logger.LogDebug($"数据上下文 {item.Key} 添加实体类型 {register.EntityType} ");
                }
                _logger.LogInformation($"数据上下文 {item.Key} 添加了 {item.Value.Length} 个实体");
            }

            _initialized = true;
        }

        /// <summary>
        /// 获取指定上下文类型的实体配置注册信息
        /// </summary>
        /// <param name="dbContextType">数据上下文类型</param>
        /// <returns></returns>
        public virtual IEntityRegister[] GetEntityRegisters(Type dbContextType)
        {
            if (!_initialized)
            {
                throw new OsharpException("数据访问模块未初始化，请确认数据上下文配置节点 OSharp:DbContexts 与要使用的数据库类型是否匹配");
            }
            return _entityRegistersDict.ContainsKey(dbContextType) ? _entityRegistersDict[dbContextType] : Array.Empty<IEntityRegister>();
        }

        /// <summary>
        /// 获取 实体类所属的数据上下文类
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>数据上下文类型</returns>
        public virtual Type GetDbContextTypeForEntity(Type entityType)
        {
            if (!_initialized)
            {
                throw new OsharpException("数据访问模块未初始化，请确认数据上下文配置节点 OSharp:DbContexts 与要使用的数据库类型是否匹配");
            }
            var dict = _entityRegistersDict;
            if (dict.IsEmpty)
            {
                throw new OsharpException("未发现任何数据上下文实体映射配置，请通过对各个实体继承基类“EntityTypeConfigurationBase<TEntity, TKey>”以使实体加载到上下文中");
            }

            foreach (var item in _entityRegistersDict)
            {
                if (item.Value.Any(m => m.EntityType == entityType))
                {
                    _logger.LogDebug($"由实体类 {entityType} 获取到所属上下文类型：{item.Key}");
                    return item.Key;
                }
            }

            throw new OsharpException($"无法获取实体类 {entityType} 的所属上下文类型，请通过继承基类“EntityTypeConfigurationBase<TEntity, TKey>”配置实体加载到上下文中");
        }


        private class EntityInfoConfiguration : EntityTypeConfigurationBase<EntityInfo, Guid>
        {
            /// <summary>
            /// 重写以实现实体类型各个属性的数据库配置
            /// </summary>
            /// <param name="builder">实体类型创建器</param>
            public override void Configure(EntityTypeBuilder<EntityInfo> builder)
            {
#if NET5_0_OR_GREATER
                builder.HasIndex(m => m.TypeName).HasDatabaseName("ClassFullNameIndex").IsUnique();
#else
                builder.HasIndex(m => m.TypeName).HasName("ClassFullNameIndex").IsUnique();
#endif
            }
        }


        private class FunctionConfiguration : EntityTypeConfigurationBase<Function, Guid>
        {
            /// <summary>
            /// 重写以实现实体类型各个属性的数据库配置
            /// </summary>
            /// <param name="builder">实体类型创建器</param>
            public override void Configure(EntityTypeBuilder<Function> builder)
            {
#if NET5_0_OR_GREATER
                builder.HasIndex(m => new { m.Area, m.Controller, m.Action }).HasDatabaseName("AreaControllerActionIndex").IsUnique();
#else
                builder.HasIndex(m => new { m.Area, m.Controller, m.Action }).HasName("AreaControllerActionIndex").IsUnique();
#endif
            }
        }


        private class KeyValueConfiguration : EntityTypeConfigurationBase<KeyValue, Guid>
        {
            /// <summary>
            /// 重写以实现实体类型各个属性的数据库配置
            /// </summary>
            /// <param name="builder">实体类型创建器</param>
            public override void Configure(EntityTypeBuilder<KeyValue> builder)
            {
                builder.Property(m => m.ValueJson).HasColumnType("text");
#if NET5_0_OR_GREATER
                builder.HasIndex(m => m.Key).HasDatabaseName("KeyIndex").IsUnique();
#else
                builder.HasIndex(m => m.Key).HasName("KeyIndex").IsUnique();
#endif
            }
        }
    }
}