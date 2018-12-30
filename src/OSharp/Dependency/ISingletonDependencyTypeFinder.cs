// -----------------------------------------------------------------------
//  <copyright file="ISingletonDependencyTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-31 0:12</last-date>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

using OSharp.Reflection;


namespace OSharp.Dependency
{
    /// <summary>
    /// 定义 <see cref="ServiceLifetime.Singleton"/>生命周期类型的服务映射类型查找器
    /// </summary>
    public interface ISingletonDependencyTypeFinder : ITypeFinder
    { }
}