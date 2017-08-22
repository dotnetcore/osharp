// -----------------------------------------------------------------------
//  <copyright file="IExpirable.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-19 2:33</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义可过期性，包含生效时间和过期时间
    /// </summary>
    public interface IExpirable
    {
        /// <summary>
        /// 获取或设置 生效时间
        /// </summary>
        DateTime? BeginTime { get; set; }

        /// <summary>
        /// 获取或设置 过期时间
        /// </summary>
        DateTime? EndTime { get; set; }
    }
}