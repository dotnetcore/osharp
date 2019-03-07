// -----------------------------------------------------------------------
//  <copyright file="EntityRegisterManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-03-08 3:07</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Collections;
using OSharp.Core.EntityInfos;
using OSharp.Core.Functions;
using OSharp.Dependency;
using OSharp.Exceptions;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体管理器
    /// </summary>
    [Dependency(ServiceLifetime.Singleton, TryAdd = true)]
    public class EntityManager : IEntityManager
    {
        private readonly ConcurrentDictionary<Type, IEntityRegister[]> _entityRegistersDict
            = new ConcurrentDictionary<Type, IEntityRegister[]>();
        private readonly IEntityConfigurationTypeFinder _typeFinder;
        private bool _initialized;

        /// <summary>
        /// 初始化一个<see cref="EntityManager"/>类型的新实例
        /// </summary>
        public EntityManager(IEntityConfigurationTypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }

        /// <summary>
        /// 初始化实体类型注册
        /// </summary>
        public virtual void Initialize()
        {
            var dict = _entityRegistersDict;
            Type[] types = _typeFinder.FindAll(true);
            if (types.Length == 0 || _initialized)
            {
                return;
            }

            dict.Clear();
            List<IEntityRegister> registers = types.Select(type => Activator.CreateInstance(type) as IEntityRegister).ToList();
            List<IGrouping<Type, IEntityRegister>> groups = registers.GroupBy(m => m.DbContextType).ToList();
            Type key;
            foreach (IGrouping<Type, IEntityRegister> group in groups)
            {
                key = group.Key ?? typeof(DefaultDbContext);
                List<IEntityRegister> list = new List<IEntityRegister>();
                if (group.Key == null || group.Key == typeof(DefaultDbContext))
                {
                    list.AddRange(group);
                }
                else
                {
                    list = group.ToList();
                }

                if (list.Count > 0)
                {
                    dict[key] = list.ToArray();
                }
            }

            //添加框架的一些默认实体的实体映射信息（如果不存在）
            key = typeof(DefaultDbContext);
            if (dict.ContainsKey(key))
            {
                List<IEntityRegister> list = dict[key].ToList();
                list.AddIfNotExist(new EntityInfoConfiguration(), m => m.EntityType.IsBaseOn<IEntityInfo>());
                list.AddIfNotExist(new FunctionConfiguration(), m => m.EntityType.IsBaseOn<IFunction>());
                dict[key] = list.ToArray();
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
            return _entityRegistersDict.ContainsKey(dbContextType) ? _entityRegistersDict[dbContextType] : new IEntityRegister[0];
        }

        /// <summary>
        /// 获取 实体类所属的数据上下文类
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>数据上下文类型</returns>
        public virtual Type GetDbContextTypeForEntity(Type entityType)
        {
            var dict = _entityRegistersDict;
            if (dict.Count == 0)
            {
                throw new OsharpException($"未发现任何数据上下文实体映射配置，请通过对各个实体继承基类“EntityTypeConfigurationBase<TEntity, TKey>”以使实体加载到上下文中");
            }

            foreach (var item in _entityRegistersDict)
            {
                if (item.Value.Any(m => m.EntityType == entityType))
                {
                    return item.Key;
                }
            }

            throw new OsharpException($"无法获取实体类“{entityType}”的所属上下文类型，请通过继承基类“EntityTypeConfigurationBase<TEntity, TKey>”配置实体加载到上下文中");
        }


        private class EntityInfoConfiguration : EntityTypeConfigurationBase<EntityInfo, Guid>
        {
            /// <summary>
            /// 重写以实现实体类型各个属性的数据库配置
            /// </summary>
            /// <param name="builder">实体类型创建器</param>
            public override void Configure(EntityTypeBuilder<EntityInfo> builder)
            {
                builder.HasIndex(m => m.TypeName).HasName("ClassFullNameIndex").IsUnique();
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
                builder.HasIndex(m => new { m.Area, m.Controller, m.Action }).HasName("AreaControllerActionIndex").IsUnique();
            }
        }
    }
}