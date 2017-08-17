// -----------------------------------------------------------------------
//  <copyright file="EntityConfigurationTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-17 2:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;

using OSharp.Finders;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体类配置类型查找器
    /// </summary>
    public class EntityConfigurationTypeFinder : FinderBase<Type>, IEntityConfigurationTypeFinder
    {
        private readonly IEntityConfigurationAssemblyFinder _assemblyFinder;
        private readonly ReadOnlyDictionary<Type,>

        /// <summary>
        /// 初始化一个<see cref="EntityConfigurationTypeFinder"/>类型的新实例
        /// </summary>
        public EntityConfigurationTypeFinder(IEntityConfigurationAssemblyFinder assemblyFinder)
        {
            _assemblyFinder = assemblyFinder;
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
            return types;
        }

        /// <summary>
        /// 获取指定上下文类型的实体配置注册信息
        /// </summary>
        /// <param name="dbContextType">数据上下文类型</param>
        /// <returns></returns>
        public IEntityRegister[] GetEntityRegisters(Type dbContextType)
        {
            throw new NotImplementedException();
        }
    }
}