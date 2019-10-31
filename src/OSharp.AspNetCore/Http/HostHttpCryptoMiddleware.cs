// -----------------------------------------------------------------------
//  <copyright file="HostHttpCryptoMiddleware.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-30 22:06</last-date>
// -----------------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;


namespace OSharp.AspNetCore.Http
{
    /// <summary>
    /// 服务端通信加密解密中间件，对请求进行解密，对响应进行加密
    /// </summary>
    public class HostHttpCryptoMiddleware : IMiddleware
    {
        private readonly IHostHttpCrypto _hostHttpCrypto;

        /// <summary>
        /// 初始化一个<see cref="HostHttpCryptoMiddleware"/>类型的新实例
        /// </summary>
        public HostHttpCryptoMiddleware(IHostHttpCrypto hostHttpCrypto)
        {
            _hostHttpCrypto = hostHttpCrypto;
        }

        /// <summary>Request handling method.</summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the current request.</param>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the execution of this middleware.</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            HttpRequest request = context.Request;
            await _hostHttpCrypto.DecryptRequest(request);
            await next(context);
            HttpResponse response = context.Response;
            await _hostHttpCrypto.EncryptResponse(response);
        }
    }
}