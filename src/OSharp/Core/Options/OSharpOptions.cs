// -----------------------------------------------------------------------
//  <copyright file="OSharpOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-03 0:57</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;


namespace OSharp.Core.Options
{
    /// <summary>
    /// OSharp框架配置选项信息
    /// </summary>
    public class OSharpOptions
    {
        /// <summary>
        /// 初始化一个<see cref="OSharpOptions"/>类型的新实例
        /// </summary>
        public OSharpOptions()
        {
            DbContextOptionses = new ConcurrentDictionary<string, OSharpDbContextOptions>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 获取 数据上下文配置信息
        /// </summary>
        public IDictionary<string, OSharpDbContextOptions> DbContextOptionses { get; }
    }
}