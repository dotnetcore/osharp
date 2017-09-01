// -----------------------------------------------------------------------
//  <copyright file="IMapTargetTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-25 1:03</last-date>
// -----------------------------------------------------------------------

using OSharp.Dependency;
using OSharp.Reflection;


namespace OSharp.Mapping
{
    /// <summary>
    /// 定义对象映射目标类型查找器
    /// </summary>
    [IgnoreDependency]
    public interface IMapTargetTypeFinder : ITypeFinder
    { }
}