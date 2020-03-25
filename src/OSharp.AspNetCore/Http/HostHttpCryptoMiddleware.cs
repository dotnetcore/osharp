// -----------------------------------------------------------------------
//  <copyright file="HostHttpCryptoMiddleware.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-30 22:06</last-date>
// -----------------------------------------------------------------------

using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;


namespace OSharp.AspNetCore.Http
{
    /// <summary>
    /// 服务端通信加密解密中间件，对请求进行解密，对响应进行加密，如使用，请将此中间件放在第一个
    /// </summary>
    public class HostHttpCryptoMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostHttpCrypto _hostHttpCrypto;

        /// <summary>
        /// 初始化一个<see cref="HostHttpCryptoMiddleware"/>类型的新实例
        /// </summary>
        public HostHttpCryptoMiddleware(RequestDelegate next, IHostHttpCrypto hostHttpCrypto)
        {
            _next = next;
            _hostHttpCrypto = hostHttpCrypto;
        }

        /// <summary>
        /// 执行中间件拦截逻辑
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            HttpRequest request = context.Request;
            await _hostHttpCrypto.DecryptRequest(request);
            await _next(context);
            HttpResponse response = context.Response;
            await _hostHttpCrypto.EncryptResponse(response);
        }
    }
}