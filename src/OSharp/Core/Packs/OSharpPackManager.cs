// -----------------------------------------------------------------------
//  <copyright file="OsharpPackManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:18</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Collections;
using OSharp.Core.Builders;
using OSharp.Dependency;
using OSharp.Exceptions;


namespace OSharp.Core.Packs
{
    /// <summary>
    /// OSharp模块管理器
    /// </summary>
    public class OsharpPackManager : IOsharpPackManager
    {
        private readonly List<OsharpPack> _sourcePacks;

        /// <summary>
        /// 初始化一个<see cref="OsharpPackManager"/>类型的新实例
        /// </summary>
        public OsharpPackManager()
        {
            _sourcePacks = new List<OsharpPack>();
            LoadedPacks = new List<OsharpPack>();
        }

        /// <summary>
        /// 获取 自动检索到的所有模块信息
        /// </summary>
        public IEnumerable<OsharpPack> SourcePacks => _sourcePacks;

        /// <summary>
        /// 获取 最终加载的模块信息集合
        /// </summary>
        public IEnumerable<OsharpPack> LoadedPacks { get; private set; }

        /// <summary>
        /// 加载模块服务
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <returns></returns>
        public virtual IServiceCollection LoadPacks(IServiceCollection services)
        {
            IOsharpPackTypeFinder packTypeFinder =
                services.GetOrAddTypeFinder<IOsharpPackTypeFinder>(assemblyFinder => new OsharpPackTypeFinder(assemblyFinder));
            Type[] packTypes = packTypeFinder.FindAll();
            _sourcePacks.Clear();
            _sourcePacks.AddRange(packTypes.Select(m => (OsharpPack)Activator.CreateInstance(m)));

            IOsharpBuilder builder = services.GetSingletonInstance<IOsharpBuilder>();
            List<OsharpPack> packs;
            if (builder.AddPacks.Any())
            {
                packs = _sourcePacks.Where(m => m.Level == PackLevel.Core)
                    .Union(_sourcePacks.Where(m => builder.AddPacks.Contains(m.GetType()))).Distinct()
                    .OrderBy(m => m.Level).ThenBy(m => m.Order).ToList();
                List<OsharpPack> dependPacks = new List<OsharpPack>();
                foreach (OsharpPack pack in packs)
                {
                    Type[] dependPackTypes = pack.GetDependPackTypes();
                    foreach (Type dependPackType in dependPackTypes)
                    {
                        OsharpPack dependPack = _sourcePacks.Find(m => m.GetType() == dependPackType);
                        if (dependPack == null)
                        {
                            throw new OsharpException($"加载模块{pack.GetType().FullName}时无法找到依赖模块{dependPackType.FullName}");
                        }
                        dependPacks.AddIfNotExist(dependPack);
                    }
                }
                packs = packs.Union(dependPacks).Distinct().ToList();
            }
            else
            {
                packs = _sourcePacks.ToList();
                packs.RemoveAll(m => builder.ExceptPacks.Contains(m.GetType()));
            }

            // 按先层级后顺序的规则进行排序
            packs = packs.OrderBy(m => m.Level).ThenBy(m => m.Order).ToList();
            LoadedPacks = packs;
            foreach (OsharpPack pack in LoadedPacks)
            {
                services = pack.AddServices(services);
            }

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public virtual void UsePack(IServiceProvider provider)
        {
            ILogger logger = provider.GetLogger<OsharpPackManager>();
            logger.LogInformation("OSharp框架初始化开始");
            DateTime dtStart = DateTime.Now;

            foreach (OsharpPack pack in LoadedPacks)
            {
                pack.UsePack(provider);
                logger.LogInformation($"模块{pack.GetType()}加载成功");
            }

            TimeSpan ts = DateTime.Now.Subtract(dtStart);
            logger.LogInformation($"Osharp框架初始化完成，耗时：{ts:g}");
        }
    }
}