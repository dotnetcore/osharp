// -----------------------------------------------------------------------
//  <copyright file="SingletonAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-28 15:00</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Wpf.Stylet
{
    /// <summary>
    /// 标注Stylet的单例视图模型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonAttribute : Attribute
    { }
}