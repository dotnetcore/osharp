// -----------------------------------------------------------------------
//  <copyright file="IMethodInfoFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 23:26</last-date>
// -----------------------------------------------------------------------

using System.Reflection;

using OSharp.Finders;


namespace OSharp.Reflection
{
    /// <summary>
    /// 定义方法信息查找器
    /// </summary>
    public interface IMethodInfoFinder : IFinder<MemberInfo>
    { }
}