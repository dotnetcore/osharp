// -----------------------------------------------------------------------
//  <copyright file="HttpClientExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 23:39</last-date>
// -----------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace OSharp.Http
{
    /// <summary>
    /// HTTP 扩展方法
    /// </summary>
    public static class HttpExtensions
    {
        /// <summary>
        /// Patch请求
        /// </summary>
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string url, HttpContent content)
        {
            return client.PatchAsync(new Uri(url), content);
        }

        /// <summary>
        /// Patch请求
        /// </summary>
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri uri, HttpContent content)
        {
            HttpMethod method = new HttpMethod("PATCH");
            HttpRequestMessage request = new HttpRequestMessage(method, uri) { Content = content };
            HttpResponseMessage response = await client.SendAsync(request);
            return response;
        }

        /// <summary>
        /// 由旧的<see cref="HttpRequestMessage"/>和新数据创建新的<see cref="HttpRequestMessage"/>
        /// </summary>
        public static HttpRequestMessage CreateNew(this HttpRequestMessage request, string data)
        {
            HttpContent content = new StringContent(data);
            foreach (var header in request.Headers)
            {
                content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            request.Content = content;
            return request;
        }

        /// <summary>
        /// 由旧的<see cref="HttpResponseMessage"/>和新数据创建新的<see cref="HttpResponseMessage"/>
        /// </summary>
        public static HttpResponseMessage CreateNew(this HttpResponseMessage response, string data)
        {
            HttpContent content = new StringContent(data);
            foreach (var header in response.Content.Headers)
            {
                content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            response.Content = content;
            return response;
        }

    }
}