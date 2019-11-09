// -----------------------------------------------------------------------
//  <copyright file="ClientHttpCrypto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-30 23:38</last-date>
// -----------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Core.Options;
using OSharp.Dependency;
using OSharp.Exceptions;
using OSharp.Properties;
using OSharp.Security;


namespace OSharp.Http
{
    /// <summary>
    /// Http客户端通信加密解密器
    /// </summary>
    [Dependency(ServiceLifetime.Transient, TryAdd = true)]
    public class ClientHttpCrypto : IClientHttpCrypto
    {
        private readonly ILogger _logger;
        private readonly TransmissionEncryptor _encryptor;
        private readonly string _publicKey;

        /// <summary>
        /// 初始化一个<see cref="ClientHttpCrypto"/>类型的新实例
        /// </summary>
        public ClientHttpCrypto(IServiceProvider provider)
        {
            _logger = provider.GetLogger(typeof(ClientHttpCrypto));
            OsharpOptions options = provider.GetOSharpOptions();
            if (options.HttpEncrypt?.Enabled == true)
            {
                HttpEncryptOptions httpEncrypt = options.HttpEncrypt;
                string clientPublicKey = httpEncrypt.ClientPublicKey;
                if (string.IsNullOrEmpty(clientPublicKey))
                {
                    throw new OsharpException("配置文件中HttpEncrypt节点的ClientPublicKey不能为空");
                }
                RsaHelper rsa = new RsaHelper();
                _encryptor = new TransmissionEncryptor(rsa.PrivateKey, httpEncrypt.ClientPublicKey);
                _publicKey = rsa.PublicKey;
            }
        }

        /// <summary>
        /// 将要发往服务器的请求进行加密
        /// </summary>
        /// <param name="request">未加密的请求</param>
        /// <returns>加密后的请求</returns>
        public virtual async Task<HttpRequestMessage> EncryptRequest(HttpRequestMessage request)
        {
            if (_encryptor == null || string.IsNullOrEmpty(_publicKey) || request.Method == HttpMethod.Get || request.Content == null)
            {
                return request;
            }

            string data = await request.Content.ReadAsStringAsync();
            data = _encryptor.EncryptData(data);
            request = request.CreateNew(data);
            request.Headers.Add(HttpHeaderNames.ClientPublicKey, _publicKey);
            return request;
        }

        /// <summary>
        /// 解密从服务器收到的响应
        /// </summary>
        /// <param name="response">加密的响应</param>
        /// <returns>解密后的响应</returns>
        public virtual async Task<HttpResponseMessage> DecryptResponse(HttpResponseMessage response)
        {
            if (_encryptor == null || !response.IsSuccessStatusCode)
            {
                return response;
            }

            string data = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(data))
            {
                return response;
            }

            try
            {
                data = _encryptor.DecryptAndVerifyData(data);
                if (data == null)
                {
                    throw new OsharpException(Resources.Http_Security_Client_VerifyResponse_Failt);
                }
                response = response.CreateNew(data);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(Resources.Http_Seciruty_Client_DecryptResponse_Failt, ex);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return response;
            }
        }
    }
}