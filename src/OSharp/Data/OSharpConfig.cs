// -----------------------------------------------------------------------
//  <copyright file="OSharpConfig.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-22 10:04</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;
using OSharp.Entity;
using OSharp.Exceptions;


namespace OSharp.Data
{
    /// <summary>
    /// OSharp框架配置信息
    /// </summary>
    public class OsharpConfig
    {
        /// <summary>
        /// 初始化一个<see cref="OsharpConfig"/>类型的新实例
        /// </summary>
        public OsharpConfig()
        {
            DbContexts = new Dictionary<string, OsharpDbContextConfig>();
        }

        /// <summary>
        /// 获取或设置 数据上下文配置信息
        /// </summary>
        public IDictionary<string, OsharpDbContextConfig> DbContexts { get; set; }
    }

    /// <summary>
    /// OSharp数据上下文配置
    /// </summary>
    public class OsharpDbContextConfig
    {
        /// <summary>
        /// 获取 上下文类型
        /// </summary>
        public Type DbContextType => string.IsNullOrEmpty(DbContextTypeName) ? null : Type.GetType(DbContextTypeName);

        /// <summary>
        /// 获取或设置 上下文类型全名
        /// </summary>
        public string DbContextTypeName { get; set; }

        /// <summary>
        /// 获取或设置 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 获取或设置 数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; set; }
    }
}