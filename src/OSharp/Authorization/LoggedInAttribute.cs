// -----------------------------------------------------------------------
//  <copyright file="LoginedAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-15 2:44</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Authorization;

/// <summary>
/// 指定功能需要登录才能访问
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class LoggedInAttribute : Attribute
{ }
