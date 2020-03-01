// -----------------------------------------------------------------------
//  <copyright file="IdentityServerOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-21 14:56</last-date>
// -----------------------------------------------------------------------

namespace OSharp.IdentityServer.Options
{
    /// <summary>
    /// IdentityServer选项
    /// </summary>
    public class IdentityServerOptions
    {
        /// <summary>
        /// 获取或设置 是否允许Token清除
        /// </summary>
        public bool EnableTokenCleanup { get; set; } = false;

        /// <summary>
        /// 获取或设置 Token清除间隔，单位秒，默认3600(1小时)
        /// </summary>
        public int TokenCleanupInterval { get; set; } = 3600;

        /// <summary>
        /// 获取或设置 每次清除Token的数量，默认100
        /// </summary>
        public int TokenCleanupBatchSize { get; set; } = 100;

        /// <summary>
        /// 获取或设置 IdentityServer4的上下文类型路径，默认为null
        /// </summary>
        public string DbContextTypeName { get; set; }
    }
}