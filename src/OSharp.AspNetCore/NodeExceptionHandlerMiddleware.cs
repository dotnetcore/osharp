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

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using OSharp.AspNetCore.UI;
using OSharp.Data;
using OSharp.Json;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// Node技术异常处理中间件
    /// </summary>
    public class NodeExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<NodeExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// 初始化一个<see cref="NodeExceptionHandlerMiddleware"/>类型的新实例
        /// </summary>
        public NodeExceptionHandlerMiddleware(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NodeExceptionHandlerMiddleware>();
        }

        /// <summary>Request handling method.</summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the current request.</param>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the execution of this middleware.</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, ex.Message);
                if (context.Request.IsAjaxRequest() || context.Request.IsJsonContextType())
                {
                    if (context.Response.HasStarted)
                    {
                        return;
                    }
                    context.Response.StatusCode = 500;
                    context.Response.Clear();
                    context.Response.ContentType = "application/json; charset=utf-8";
                    await context.Response.WriteAsync(new AjaxResult(ex.Message, AjaxResultType.Error).ToJsonString());
                    return;
                }
                throw;
            }
        }
    }
}