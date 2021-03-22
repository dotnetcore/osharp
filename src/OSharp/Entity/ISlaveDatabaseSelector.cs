// -----------------------------------------------------------------------
//  <copyright file="ISlaveDatabaseSelector.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-21 0:14</last-date>
// -----------------------------------------------------------------------

using OSharp.Core.Options;
using OSharp.Dependency;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义从数据库选择功能
    /// </summary>
    [MultipleDependency]
    public interface ISlaveDatabaseSelector
    {
        /// <summary>
        /// 获取 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 从所有从数据库中返回一个
        /// </summary>
        /// <param name="slaves">所有从数据库</param>
        /// <returns></returns>
        SlaveDatabaseOptions Select(SlaveDatabaseOptions[] slaves);
    }
}