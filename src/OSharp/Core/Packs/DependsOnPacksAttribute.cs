// -----------------------------------------------------------------------
//  <copyright file="DependsOnPacksAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:19</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Core.Packs
{
    /// <summary>
    /// 定义OSharp模块依赖
    /// </summary>
    public class DependsOnPacksAttribute : Attribute
    {
        /// <summary>
        /// 初始化一个 OSharp模块依赖<see cref="DependsOnPacksAttribute"/>类型的新实例
        /// </summary>
        public DependsOnPacksAttribute(params Type[] dependedPackTypes)
        {
            DependedPackTypes = dependedPackTypes;
        }

        /// <summary>
        /// 获取 当前模块的依赖模块类型集合
        /// </summary>
        public Type[] DependedPackTypes { get; }
    }
}