// -----------------------------------------------------------------------
//  <copyright file="DependsOnAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-07 21:48</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Core.Modules
{
    /// <summary>
    /// 定义OSharp模块依赖
    /// </summary>
    public class DependsOnModulesAttribute : Attribute
    {
        /// <summary>
        /// 初始化一个 OSharp模块依赖<see cref="DependsOnModulesAttribute"/>类型的新实例
        /// </summary>
        public DependsOnModulesAttribute(params Type[] dependedModuleTypes)
        {
            DependedModuleTypes = dependedModuleTypes;
        }

        /// <summary>
        /// 获取 当前模块的依赖模块类型集合
        /// </summary>
        public Type[] DependedModuleTypes { get; }
    }
}