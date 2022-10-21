// -----------------------------------------------------------------------
//  <copyright file="HttpClientExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 23:39</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using OSharp.Extensions;


namespace OSharp.Http
{
    /// <summary>
    /// HTTP 扩展方法
    /// </summary>
    public static class HttpExtensions
    {
        /// <summary>
        /// 基于Json的Get请求
        /// </summary>
        public static async Task<TResult> GetAsync<TResult>(this HttpClient client, string url)
            where TResult : class
        {
            string json = await client.GetStringAsync(url);
            if (typeof(TResult) == typeof(string))
            {
                return json as TResult;
            }
            return JsonConvert.DeserializeObject<TResult>(json);
        }

        /// <summary>
        /// 基于Json的Post请求
        /// </summary>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string url, JsonContent content)
        {
            return client.PostAsync(url, content);
        }

        /// <summary>
        /// 基于Json的Post请求
        /// </summary>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string url, object data)
        {
            return client.PostAsync(url, new JsonContent(data));
        }

        /// <summary>
        /// 基于Json的Post请求
        /// </summary>
        public static async Task<TResult> PostAsync<TResult>(this HttpClient client, string url, object data)
            where TResult : class
        {
            HttpResponseMessage response = await client.PostAsync(url, data);
            if (!response.IsSuccessStatusCode)
            {
                return default(TResult);
            }
            string json = await response.Content.ReadAsStringAsync();
            if (typeof(TResult) == typeof(string))
            {
                return json as TResult;
            }
            return JsonConvert.DeserializeObject<TResult>(json);
        }

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

        /// <summary>
        /// 设置HttpClient的Headers值，如存在，则先移除
        /// </summary>
        public static void Set(this HttpRequestHeaders headers, string name, string value)
        {
            if (headers.Contains(name))
            {
                headers.Remove(name);
            }
            try
            {
                headers.Add(name, value);
            }
            catch (FormatException)
            {
                headers.TryAddWithoutValidation(name, value);
            }
        }

        /// <summary>
        /// 按指定编码读取<see cref="HttpContent"/>为字符串
        /// </summary>
        public static async Task<string> ReadAsStringAsync(this HttpContent content, Encoding encoding)
        {
            byte[] bytes = await content.ReadAsByteArrayAsync();
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 将<see cref="HttpContent"/>读取为GB2312字符串
        /// </summary>
        public static async Task<string> ReadAsGb2312StringAsync(this HttpContent content)
        {
            Encoding encoding = Encoding.GetEncoding("GB2312");
            return await content.ReadAsStringAsync(encoding);
        }

        /// <summary>
        /// 尝试从<see cref="HttpResponseMessage"/>中提取Cookie值
        /// </summary>
        public static bool TryGetCookie(this HttpResponseMessage response, string name, out string value)
        {
            return response.Headers.TryGetCookie(name, out value);
        }

        /// <summary>
        /// 尝试从<see cref="HttpResponseHeaders"/>中提取Cookie值
        /// </summary>
        public static bool TryGetCookie(this HttpResponseHeaders headers, string name, out string value)
        {
            value = string.Empty;
            bool flag = headers.TryGetValues("Set-Cookie", out IEnumerable<string> cookies);
            if (!flag)
            {
                return false;
            }
            value = cookies.Where(m => m.Contains($"{name}=")).Select(m => m.Substring($"{name}=", ";", "")).FirstOrDefault();
            return !value.IsNullOrEmpty();
        }

        /// <summary>
        /// 尝试从<see cref="HttpResponseMessage"/>中提取Refresh的URL
        /// </summary>
        public static bool TryGetRefreshUrl(this HttpResponseMessage response, out string url)
        {
            return response.Headers.TryGetRefreshUrl(out url);
        }

        /// <summary>
        /// 尝试从<see cref="HttpResponseHeaders"/>中提取Refresh的URL
        /// </summary>
        public static bool TryGetRefreshUrl(this HttpResponseHeaders headers, out string url)
        {
            url = null;
            if (!headers.TryGetValues("Refresh", out IEnumerable<string> values))
            {
                return false;
            }
            url = values.First().Substring("url=", "");
            return true;
        }

        /// <summary>
        /// 从Uri中获取HostUrl，形如：https://www.baidu.com/
        /// </summary>
        public static string GetHostUrl(this Uri uri)
        {
            return $"{uri.Scheme}://{uri.Host}/";
        }
    }
}