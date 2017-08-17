// -----------------------------------------------------------------------
//  <copyright file="EntityConfigurationTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-17 2:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using OSharp.Entity.Defaults;
using OSharp.Finders;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体类配置类型查找器
    /// </summary>
    public class EntityConfigurationTypeFinder : FinderBase<Type>, IEntityConfigurationTypeFinder
    {
        private readonly IEntityConfigurationAssemblyFinder _assemblyFinder;
        private ReadOnlyDictionary<Type, IEntityRegister[]> _entityRegistersDict;

        /// <summary>
        /// 初始化一个<see cref="EntityConfigurationTypeFinder"/>类型的新实例
        /// </summary>
        public EntityConfigurationTypeFinder(IEntityConfigurationAssemblyFinder assemblyFinder)
        {
            _assemblyFinder = assemblyFinder;
            _entityRegistersDict = new ReadOnlyDictionary<Type, IEntityRegister[]>(new Dictionary<Type, IEntityRegister[]>());
        }

        /// <summary>
        /// 重写以实现所有项的查找
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            Type baseType = typeof(IEntityRegister);
            Type[] types = _assemblyFinder.FindAll()
                .SelectMany(assembly => assembly.GetTypes().Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract))
                .ToArray();
            EntityRegistersInit(types);
            return types;
        }

        /// <summary>
        /// 初始化实体映射对象字典
        /// </summary>
        /// <param name="types"></param>
        private void EntityRegistersInit(Type[] types)
        {
            if (types.Length == 0)
            {
                return;
            }
            List<IEntityRegister> registers = types.Select(type => Activator.CreateInstance<IEntityRegister>()).ToList();
            Dictionary<Type, IEntityRegister[]> dict = new Dictionary<Type, IEntityRegister[]>();
            List<IGrouping<Type, IEntityRegister>> groups = registers.GroupBy(m => m.DbContextType).ToList();
            foreach (IGrouping<Type, IEntityRegister> group in groups)
            {
                Type key = group.Key ?? typeof(DefaultDbContext);
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
            _entityRegistersDict = new ReadOnlyDictionary<Type, IEntityRegister[]>(dict);
        }

        /// <summary>
        /// 获取指定上下文类型的实体配置注册信息
        /// </summary>
        /// <param name="dbContextType">数据上下文类型</param>
        /// <returns></returns>
        public IEntityRegister[] GetEntityRegisters(Type dbContextType)
        {
            return _entityRegistersDict.ContainsKey(dbContextType) ? _entityRegistersDict[dbContextType] : new IEntityRegister[0];
        }
    }
}