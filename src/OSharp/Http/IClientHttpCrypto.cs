// -----------------------------------------------------------------------
//  <copyright file="IClientHttpCrypto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-30 21:42</last-date>
// -----------------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;


namespace OSharp.Http
{
    /// <summary>
    /// 定义Http客户端加密解密功能
    /// </summary>
    public interface IClientHttpCrypto
    {
        /// <summary>
        /// 将要发往服务器的请求进行加密
        /// </summary>
        /// <param name="request">未加密的请求</param>
        /// <returns>加密后的请求</returns>
        Task<HttpRequestMessage> EncryptRequest(HttpRequestMessage request);

        /// <summary>
        /// 解密从服务器收到的响应
        /// </summary>
        /// <param name="response">加密的响应</param>
        /// <returns>解密后的响应</returns>
        Task<HttpResponseMessage> DecryptResponse(HttpResponseMessage response);
    }
}