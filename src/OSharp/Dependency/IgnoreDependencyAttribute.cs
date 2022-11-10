// -----------------------------------------------------------------------
//  <copyright file="IgnoreDependencyAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-11 0:20</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Dependency;

/// <summary>
/// 标注了此特性的类，将忽略依赖注入自动映射
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class IgnoreDependencyAttribute : Attribute
{ }