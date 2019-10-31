// -----------------------------------------------------------------------
//  <copyright file="IHostHttpCrypto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-31 2:53</last-date>
// -----------------------------------------------------------------------

using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;


namespace OSharp.AspNetCore.Http
{
    /// <summary>
    /// 定义Http服务端加密解密功能
    /// </summary>
    public interface IHostHttpCrypto
    {
        /// <summary>
        /// 将收到的客户端请求进行解密
        /// </summary>
        /// <param name="request">加密的请求</param>
        /// <returns>解密后的请求</returns>
        Task<HttpRequest> DecryptRequest(HttpRequest request);

        /// <summary>
        /// 加密发往客户端的响应
        /// </summary>
        /// <param name="response">未加密的响应</param>
        /// <returns>加密后的响应</returns>
        Task<HttpResponse> EncryptResponse(HttpResponse response);
    }
}