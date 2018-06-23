// -----------------------------------------------------------------------
//  <copyright file="PackLevel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:17</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Packs
{
    /// <summary>
    /// 模块级别，级别越核心，优先启动
    /// </summary>
    public enum PackLevel
    {
        /// <summary>
        /// 核心级别，表示系统的核心模块，
        /// 这些模块不涉及第三方组件，在系统运行中是不可替换的，核心模块将始终加载
        /// </summary>
        Core = 1,
        /// <summary>
        /// 框架级别，表示涉及第三方组件的基础模块
        /// </summary>
        Framework = 10,
        /// <summary>
        /// 应用级别，表示涉及应用数据的基础模块
        /// </summary>
        Application = 20,
        /// <summary>
        /// 业务级别，表示涉及真实业务处理的模块
        /// </summary>
        Business = 30
    }
}