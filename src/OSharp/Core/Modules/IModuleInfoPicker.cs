// -----------------------------------------------------------------------
//  <copyright file="IModuleInfoPicker.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 17:13</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Modules
{
    /// <summary>
    /// 定义模块信息提取器，从程序集中提取模块信息
    /// </summary>
    public interface IModuleInfoPicker
    {
        /// <summary>
        /// 从程序集收集模块信息
        /// </summary>
        /// <returns></returns>
        ModuleInfo[] Pickup();
    }
}