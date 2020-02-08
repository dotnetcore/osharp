// -----------------------------------------------------------------------
//  <copyright file="HttpExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-31 11:12</last-date>
// -----------------------------------------------------------------------

using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using OSharp.Extensions;


namespace OSharp.AspNetCore.Http
{
    /// <summary>
    /// HTTP 扩展方法
    /// </summary>
    public static class HttpExtensions
    {
        /// <summary>
        /// 读取<see cref="HttpRequest"/>的Body为字符串
        /// </summary>
        public static Task<string> ReadAsStringAsync(this HttpRequest request)
        {
            Stream original = request.Body;
            using (StreamReader reader = new StreamReader(request.Body))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                return reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// 读取<see cref="HttpResponse"/>的Body为字符串
        /// </summary>
        public static Task<string> ReadAsStringAsync(this HttpResponse response)
        {
            using (StreamReader reader = new StreamReader(response.Body))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                return reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// 设置<see cref="HttpRequest"/>的Body为指定字符串
        /// </summary>
        public static Task<HttpRequest> WriteBodyAsync(this HttpRequest request, string data)
        {
            if (request.Method == HttpMethod.Get.Method)
            {
                return Task.FromResult(request);
            }
            byte[] bytes = data.ToBytes();
            request.ContentLength = bytes.Length;
            request.Body = new MemoryStream(bytes);
            return Task.FromResult(request);
        }

        /// <summary>
        /// 设置<see cref="HttpResponse"/>的Body为指定字符串
        /// </summary>
        public static Task<HttpResponse> WriteBodyAsync(this HttpResponse response, string data)
        {
            if (data == null)
            {
                return Task.FromResult(response);
            }

            byte[] bytes = data.ToBytes();
            response.ContentLength = bytes.Length;
            response.Body = new MemoryStream(bytes);
            return Task.FromResult(response);
        }

        /// <summary>
        /// 获取一个值，该值指示 HTTP 响应是否成功。
        /// </summary>
        public static bool IsSuccessStatusCode(this HttpResponse response)
        {
            if (response.StatusCode >= 200)
            {
                return response.StatusCode <= 299;
            }

            return false;
        }

    }
}