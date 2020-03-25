// -----------------------------------------------------------------------
//  <copyright file="HttpContextCancellationTokenProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-04-09 22:52</last-date>
// -----------------------------------------------------------------------

using System.Threading;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Threading;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// 基于当前HttpContext的<see cref="IServiceScope"/>的异步任务取消标识
    /// </summary>
    public class HttpContextCancellationTokenProvider : ICancellationTokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 初始化一个<see cref="HttpContextCancellationTokenProvider"/>类型的新实例
        /// </summary>
        public HttpContextCancellationTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取 异步任务取消标识
        /// </summary>
        public CancellationToken Token => _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
    }
}