// -----------------------------------------------------------------------
//  <copyright file="NoneCancellationTokenProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-04-09 22:53</last-date>
// -----------------------------------------------------------------------

using System.Threading;


namespace OSharp.Threading
{
    /// <summary>
    /// 默认的异步任务取消标识提供者空实现
    /// </summary>
    public class NoneCancellationTokenProvider : ICancellationTokenProvider
    {
        /// <summary>
        /// 获取 异步任务取消标识
        /// </summary>
        public CancellationToken Token { get; } = CancellationToken.None;
    }
}