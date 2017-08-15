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
    /// <see cref="HttpClient"/>扩展方法
    /// </summary>
    public static class HttpClientExtensions
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
    }
}