// -----------------------------------------------------------------------
//  <copyright file="IScopeDependency.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-16 22:34</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Dependency;

/// <summary>
/// 实现此接口的类型将被注册为<see cref="ServiceLifetime.Scoped"/>模式
/// </summary>
[IgnoreDependency]
public interface IScopeDependency
{ }