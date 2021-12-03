// -----------------------------------------------------------------------
//  <copyright file="IConnectionStringProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-20 21:30</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Entity
{
    /// <summary>
    /// 数据库连接字符串提供器
    /// </summary>
    public interface IConnectionStringProvider
    {
        /// <summary>
        /// 获取指定数据上下文类型的数据库连接字符串
        /// </summary>
        /// <param name="dbContextType">数据上下文类型</param>
        /// <returns></returns>
        string GetConnectionString(Type dbContextType);
    }
}