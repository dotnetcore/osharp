// -----------------------------------------------------------------------
//  <copyright file="IOsharpPackManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-10 0:12</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Core.Packs
{
    /// <summary>
    /// 定义Osharp模块管理器
    /// </summary>
    public interface IOsharpPackManager
    {
        /// <summary>
        /// 获取 自动检索到的所有模块信息
        /// </summary>
        IEnumerable<OsharpPack> SourcePacks { get; }

        /// <summary>
        /// 获取 最终加载的模块信息集合
        /// </summary>
        IEnumerable<OsharpPack> LoadedPacks { get; }

        /// <summary>
        /// 加载模块服务
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <returns>服务容器</returns>
        IServiceCollection LoadPacks(IServiceCollection services);

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        void UsePack(IServiceProvider provider);
    }
}