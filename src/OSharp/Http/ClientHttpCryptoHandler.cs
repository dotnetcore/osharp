// -----------------------------------------------------------------------
//  <copyright file="ClientHttpCryptoHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-30 23:30</last-date>
// -----------------------------------------------------------------------

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace OSharp.Http
{
    /// <summary>
    /// HttpClient客户端加密通信处理器
    /// </summary>
    public class ClientHttpCryptoHandler : DelegatingHandler
    {
        private readonly IClientHttpCrypto _clientHttpCrypto;

        /// <summary>
        /// 初始化一个<see cref="ClientHttpCryptoHandler"/>类型的新实例
        /// </summary>
        public ClientHttpCryptoHandler(IClientHttpCrypto clientHttpCrypto)
        {
            _clientHttpCrypto = clientHttpCrypto;
        }

        /// <summary>Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.</summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> was <see langword="null" />.</exception>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request = await _clientHttpCrypto.EncryptRequest(request);
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            response = await _clientHttpCrypto.DecryptResponse(response);
            return response;
        }
    }
}