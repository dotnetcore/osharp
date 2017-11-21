// -----------------------------------------------------------------------
//  <copyright file="HttpRequestExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-01 18:11</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;


namespace OSharp.AspNetCore.Http
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// 确定指定的 HTTP 请求是否为 AJAX 请求。
        /// </summary>
        ///
        /// <returns>
        /// 如果指定的 HTTP 请求是 AJAX 请求，则为 true；否则为 false。
        /// </returns>
        /// <param name="request">HTTP 请求。</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="request"/> 参数为 null（在 Visual Basic 中为 Nothing）。</exception>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            Check.NotNull(request, nameof(request));
            if (request.Headers != null)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }
            return false;
        }

        /// <summary>
        /// 获取<see cref="HttpRequest"/>的请求数据
        /// </summary>
        /// <param name="request">请求信息</param>
        /// <param name="key">要获取数据的键名</param>
        /// <returns></returns>
        public static string Params(this HttpRequest request, string key)
        {
            if (request.Query.ContainsKey(key))
            {
                return request.Query[key];
            }
            if (request.HasFormContentType)
            {
                return request.Form[key];
            }
            return null;
        }
    }
}