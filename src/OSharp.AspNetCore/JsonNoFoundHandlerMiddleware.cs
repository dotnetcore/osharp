// -----------------------------------------------------------------------
//  <copyright file="NodeNoFoundHandlerMiddleware.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-30 22:15</last-date>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// Json前端技术404返回index.html中间件
    /// </summary>
    public class JsonNoFoundHandlerMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 初始化一个<see cref="JsonNoFoundHandlerMiddleware"/>类型的新实例
        /// </summary>
        public JsonNoFoundHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 执行中间件拦截逻辑
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
            if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value)
                && !context.Request.Path.Value.StartsWith("/api/", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.Path = "/index.html";
                await _next(context);
            }
        }
    }
}