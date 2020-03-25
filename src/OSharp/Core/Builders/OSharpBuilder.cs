// -----------------------------------------------------------------------
//  <copyright file="OsharpBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:40</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Collections;
using OSharp.Core.Options;
using OSharp.Core.Packs;
using OSharp.Data;
using OSharp.Exceptions;


namespace OSharp.Core.Builders
{
    /// <summary>
    /// OSharp构建器
    /// </summary>
    public class OsharpBuilder : IOsharpBuilder
    {
        private readonly List<OsharpPack> _source;
        private List<OsharpPack> _packs;

        /// <summary>
        /// 初始化一个<see cref="OsharpBuilder"/>类型的新实例
        /// </summary>
        public OsharpBuilder(IServiceCollection services)
        {
            Services = services;
            _packs = new List<OsharpPack>();
            _source = GetAllPacks(services);
        }

        /// <summary>
        /// 获取 服务集合
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// 获取 加载的模块集合
        /// </summary>
        public IEnumerable<OsharpPack> Packs => _packs;

        /// <summary>
        /// 获取 OSharp选项配置委托
        /// </summary>
        public Action<OsharpOptions> OptionsAction { get; private set; }

        /// <summary>
        /// 添加指定模块
        /// </summary>
        /// <typeparam name="TPack">要添加的模块类型</typeparam>
        public IOsharpBuilder AddPack<TPack>() where TPack : OsharpPack
        {
            Type type = typeof(TPack);
            if (_packs.Any(m => m.GetType() == type))
            {
                return this;
            }

            OsharpPack[] tmpPacks = new OsharpPack[_packs.Count];
            _packs.CopyTo(tmpPacks);

            OsharpPack pack = _source.FirstOrDefault(m => m.GetType() == type);
            if (pack == null)
            {
                throw new OsharpException($"类型为“{type.FullName}”的模块实例无法找到");
            }
            _packs.AddIfNotExist(pack);

            // 添加依赖模块
            Type[] dependTypes = pack.GetDependPackTypes();
            foreach (Type dependType in dependTypes)
            {
                OsharpPack dependPack = _source.Find(m => m.GetType() == dependType);
                if (dependPack == null)
                {
                    throw new OsharpException($"加载模块{pack.GetType().FullName}时无法找到依赖模块{dependType.FullName}");
                }
                _packs.AddIfNotExist(dependPack);
            }

            // 按先层级后顺序的规则进行排序
            _packs = _packs.OrderBy(m => m.Level).ThenBy(m => m.Order).ToList();

            tmpPacks = _packs.Except(tmpPacks).ToArray();
            foreach (OsharpPack tmpPack in tmpPacks)
            {
                AddPack(Services, tmpPack);
            }

            return this;
        }

        /// <summary>
        /// 添加OSharp选项配置
        /// </summary>
        /// <param name="optionsAction">OSharp操作选项</param>
        /// <returns>OSharp构建器</returns>
        public IOsharpBuilder AddOptions(Action<OsharpOptions> optionsAction)
        {
            Check.NotNull(optionsAction, nameof(optionsAction));
            OptionsAction = optionsAction;
            return this;
        }

        private static List<OsharpPack> GetAllPacks(IServiceCollection services)
        {
            IOsharpPackTypeFinder packTypeFinder =
                services.GetOrAddTypeFinder<IOsharpPackTypeFinder>(assemblyFinder => new OsharpPackTypeFinder(assemblyFinder));
            Type[] packTypes = packTypeFinder.FindAll();
            return packTypes.Select(m => (OsharpPack)Activator.CreateInstance(m)).ToList();
        }

        private static IServiceCollection AddPack(IServiceCollection services, OsharpPack pack)
        {
            Type type = pack.GetType();
            Type serviceType = typeof(OsharpPack);

            if (type.BaseType?.IsAbstract == false)
            {
                //移除多重继承的模块
                ServiceDescriptor[] descriptors = services.Where(m =>
                    m.Lifetime == ServiceLifetime.Singleton && m.ServiceType == serviceType
                    && m.ImplementationInstance?.GetType() == type.BaseType).ToArray();
                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }
            }

            if (!services.Any(m => m.Lifetime == ServiceLifetime.Singleton && m.ServiceType == serviceType && m.ImplementationInstance?.GetType() == type))
            {
                services.AddSingleton(typeof(OsharpPack), pack);
                pack.AddServices(services);
            }

            return services;
        }
    }
}