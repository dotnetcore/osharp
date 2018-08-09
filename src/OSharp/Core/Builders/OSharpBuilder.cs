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

using OSharp.Collections;
using OSharp.Core.Options;
using OSharp.Core.Packs;
using OSharp.Data;


namespace OSharp.Core.Builders
{
    /// <summary>
    /// OSharp构建器
    /// </summary>
    public class OsharpBuilder : IOsharpBuilder
    {
        /// <summary>
        /// 初始化一个<see cref="OsharpBuilder"/>类型的新实例
        /// </summary>
        public OsharpBuilder()
        {
            AddPacks = new List<Type>();
            ExceptPacks = new List<Type>();
        }

        /// <summary>
        /// 获取 加载的模块集合
        /// </summary>
        public IEnumerable<Type> AddPacks { get; private set; }

        /// <summary>
        /// 获取 排除的模块集合
        /// </summary>
        public IEnumerable<Type> ExceptPacks { get; private set; }

        /// <summary>
        /// 获取 OSharp选项配置委托
        /// </summary>
        public Action<OSharpOptions> OptionsAction { get; private set; }

        /// <summary>
        /// 添加指定模块，执行此功能后将仅加载指定的模块
        /// </summary>
        /// <typeparam name="TPack">要添加的模块类型</typeparam>
        public IOsharpBuilder AddPack<TPack>() where TPack : OsharpPack
        {
            List<Type> list = AddPacks.ToList();
            list.AddIfNotExist(typeof(TPack));
            AddPacks = list;
            return this;
        }

        /// <summary>
        /// 移除指定模块，执行此功能以从自动加载的模块中排除指定模块
        /// </summary>
        /// <typeparam name="TPack"></typeparam>
        /// <returns></returns>
        public IOsharpBuilder ExceptPack<TPack>() where TPack : OsharpPack
        {
            List<Type> list = ExceptPacks.ToList();
            list.AddIfNotExist(typeof(TPack));
            ExceptPacks = list;
            return this;
        }

        /// <summary>
        /// 添加OSharp选项配置
        /// </summary>
        /// <param name="optionsAction">OSharp操作选项</param>
        /// <returns>OSharp构建器</returns>
        public IOsharpBuilder AddOptions(Action<OSharpOptions> optionsAction)
        {
            Check.NotNull(optionsAction, nameof(optionsAction));
            OptionsAction = optionsAction;
            return this;
        }
    }
}