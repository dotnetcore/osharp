// -----------------------------------------------------------------------
//  <copyright file="IOsharpBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-09 12:22</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Core.Packs;


namespace OSharp.Core.Builders
{
    /// <summary>
    /// 定义OSharp构建器
    /// </summary>
    public interface IOsharpBuilder
    {
        /// <summary>
        /// 获取 服务集合
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// 获取 加载的模块集合
        /// </summary>
        IEnumerable<OsharpPack> Packs { get; }

        /// <summary>
        /// 获取 OSharp选项配置
        /// </summary>
        OsharpOptions Options { get; }

        /// <summary>
        /// 添加指定模块
        /// </summary>
        /// <typeparam name="TPack">要添加的模块类型</typeparam>
        IOsharpBuilder AddPack<TPack>() where TPack : OsharpPack;

        /// <summary>
        /// 添加加载的所有Pack，并可排除指定的Pack类型
        /// </summary>
        /// <param name="exceptPackTypes">要排除的Pack类型</param>
        /// <returns></returns>
        IOsharpBuilder AddPacks(params Type[] exceptPackTypes);
    }
}
