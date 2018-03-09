// -----------------------------------------------------------------------
//  <copyright file="IOSharpBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-09 12:22</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using OSharp.Core.Modules;
using OSharp.Core.Options;


namespace OSharp.Core.Builders
{
    /// <summary>
    /// 定义OSharp构建器
    /// </summary>
    public interface IOSharpBuilder
    {
        /// <summary>
        /// 获取 加载的模块集合
        /// </summary>
        IEnumerable<Type> Modules { get; }

        /// <summary>
        /// 获取 OSharp选项配置委托
        /// </summary>
        Action<OSharpOptions> OptionsAction { get; }

        /// <summary>
        /// 添加指定模块
        /// </summary>
        /// <typeparam name="TModule">要添加的模块类型</typeparam>
        IOSharpBuilder AddModule<TModule>() where TModule : OSharpModule;

        /// <summary>
        /// 添加OSharp选项配置
        /// </summary>
        /// <param name="optionsAction">OSharp操作选项</param>
        /// <returns>OSharp构建器</returns>
        IOSharpBuilder AddOptions(Action<OSharpOptions>optionsAction);
    }
}