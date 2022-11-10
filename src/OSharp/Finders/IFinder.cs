// -----------------------------------------------------------------------
//  <copyright file="IFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 23:23</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Finders;

/// <summary>
/// 定义一个查找器
/// </summary>
/// <typeparam name="TItem">要查找的项类型</typeparam>
[IgnoreDependency]
public interface IFinder<out TItem>
{
    /// <summary>
    /// 查找指定条件的项
    /// </summary>
    /// <param name="predicate">筛选条件</param>
    /// <param name="fromCache">是否来自缓存</param>
    /// <returns></returns>
    TItem[] Find(Func<TItem, bool> predicate, bool fromCache = false);

    /// <summary>
    /// 查找所有项
    /// </summary>
    /// <param name="fromCache">是否来自缓存</param>
    /// <returns></returns>
    TItem[] FindAll(bool fromCache = false);
}