// -----------------------------------------------------------------------
//  <copyright file="ModuleLevel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-20 0:57</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Modules
{
    /// <summary>
    /// 模块级别，级别越核心，优先启动
    /// </summary>
    public enum ModuleLevel
    {
        /// <summary>
        /// 核心级别
        /// </summary>
        Core = 1,
        /// <summary>
        /// 框架级别
        /// </summary>
        Framework = 10,
        /// <summary>
        /// 应用级别
        /// </summary>
        Application = 20,
        /// <summary>
        /// 业务级别
        /// </summary>
        Business =30
    }
}