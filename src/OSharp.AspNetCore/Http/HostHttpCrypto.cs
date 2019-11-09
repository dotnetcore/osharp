// -----------------------------------------------------------------------
//  <copyright file="HostHttpCrypto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-31 2:56</last-date>
// -----------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Core.Options;
using OSharp.Dependency;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Http;


namespace OSharp.AspNetCore.Http
{
    /// <summary>
    /// Http服务端加密解密功能
    /// </summary>
    [Dependency(ServiceLifetime.Singleton, TryAdd = true)]
    public class HostHttpCrypto : IHostHttpCrypto
    {
        private readonly ILogger _logger;
        private readonly string _privateKey;
        private TransmissionEncryptor _encryptor;

        /// <summary>
        /// 初始化一个<see cref="HostHttpCrypto"/>类型的新实例
        /// </summary>
        public HostHttpCrypto(IServiceProvider provider)
        {
            _logger = provider.GetLogger(typeof(ClientHttpCrypto));
            OsharpOptions options = provider.GetOSharpOptions();
            if (options?.HttpEncrypt?.Enabled == true)
            {
                HttpEncryptOptions httpEncrypt = options.HttpEncrypt;
                _privateKey = httpEncrypt.HostPrivateKey;
                if (string.IsNullOrEmpty(_privateKey))
                {
                    throw new OsharpException("配置文件中HttpEncrypt节点的HostPrivateKey不能为空");
                }
            }
        }

        /// <summary>
        /// 将收到的客户端请求进行解密
        /// </summary>
        /// <param name="request">加密的请求</param>
        /// <returns>解密后的请求</returns>
        public async Task<HttpRequest> DecryptRequest(HttpRequest request)
        {
            if (_privateKey == null || request.Method == HttpMethod.Get.Method || request.Body == null)
            {
                return request;
            }

            string publicKey = request.Headers.GetOrDefault(HttpHeaderNames.ClientPublicKey);
            if (publicKey != null)
            {
                //throw new OsharpException("在请求头中客户端公钥信息无法找到");
                _encryptor = new TransmissionEncryptor(_privateKey, publicKey);
            }

            if (_encryptor == null)
            {
                return request;
            }

            try
            {
                string data = await request.ReadAsStringAsync();
                if (string.IsNullOrEmpty(data))
                {
                    return request;
                }

                data = _encryptor.DecryptAndVerifyData(data);
                if (data == null)
                {
                    throw new OsharpException("服务器解析请求数据时发生异常");
                }

                await request.WriteBodyAsync(data);
                return request;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex is CryptographicException ? "服务器解析传输数据时发生异常。" : "服务器对请求数据进行解密处理时发生异常。", ex);
                throw new OsharpException("服务器对请求数据进行解密处理时发生异常。", ex);
            }
        }

        /// <summary>
        /// 加密发往客户端的响应
        /// </summary>
        /// <param name="response">未加密的响应</param>
        /// <returns>加密后的响应</returns>
        public async Task<HttpResponse> EncryptResponse(HttpResponse response)
        {
            if (_encryptor == null || !response.IsSuccessStatusCode())
            {
                return response;
            }

            string data = await response.ReadAsStringAsync();
            if (string.IsNullOrEmpty(data))
            {
                return response;
            }

            try
            {
                data = _encryptor.EncryptData(data);
                response = await response.WriteBodyAsync(data);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("服务器对返回数据进行加密处理时发生异常", ex);
                throw;
            }
        }
    }
}