// -----------------------------------------------------------------------
//  <copyright file="JsonExceptionHandlerMiddleware.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-12 17:51</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using OSharp.AspNetCore.Http;
using OSharp.AspNetCore.UI;


namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// Node技术异常处理中间件
    /// </summary>
    public class NodeExceptionHandlerMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<NodeExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// 初始化一个<see cref="NodeExceptionHandlerMiddleware"/>类型的新实例
        /// </summary>
        public NodeExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            Check.NotNull(next, nameof(next));

            _next = next;
            _logger = loggerFactory.CreateLogger<NodeExceptionHandlerMiddleware>();
        }

        /// <summary>
        /// 执行中间件拦截逻辑
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, ex.Message);
                if (context.Request.IsAjaxRequest() || context.Request.IsJsonContextType())
                {
                    context.Response.StatusCode = 500;
                    context.Response.Clear();
                    context.Response.ContentType = "applicaton/json";
                    await context.Response.WriteAsync(new AjaxResult(ex.Message, AjaxResultType.Error).ToJsonString());
                    return;
                }
                throw;
            }
        }
    }
}