// -----------------------------------------------------------------------
//  <copyright file="IRecurringJobFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-30 17:36</last-date>
// -----------------------------------------------------------------------

using OSharp.Reflection;


namespace OSharp.Hangfire
{
    /// <summary>
    /// 重复作业类型查找器
    /// </summary>
    public interface IRecurringJobFinder : ITypeFinder
    { }
}