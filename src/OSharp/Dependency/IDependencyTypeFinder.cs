// -----------------------------------------------------------------------
//  <copyright file="IDependencyTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-31 20:53</last-date>
// -----------------------------------------------------------------------

using OSharp.Reflection;


namespace OSharp.Dependency
{
    /// <summary>
    /// 依赖注入类型查找器，查找标注了<see cref="DependencyAttribute"/>特性，
    /// 或者<see cref="ISingletonDependency"/>,<see cref="IScopeDependency"/>,<see cref="ITransientDependency"/>三个接口的服务实现类型
    /// </summary>
    public interface IDependencyTypeFinder : ITypeFinder
    { }
}