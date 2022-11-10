// -----------------------------------------------------------------------
//  <copyright file="IAssemblyFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 23:25</last-date>
// -----------------------------------------------------------------------


namespace OSharp.Reflection;

/// <summary>
/// 定义程序集查找器
/// </summary>
[IgnoreDependency]
public interface IAssemblyFinder : IFinder<Assembly>
{ }