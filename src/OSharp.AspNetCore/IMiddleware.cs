// -----------------------------------------------------------------------
//  <copyright file="IMiddleware.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-11 8:59</last-date>
// -----------------------------------------------------------------------

using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// 定义AspNetCore中间件
    /// </summary>
    public interface IMiddleware
    {
        /// <summary>
        /// 执行中间件拦截逻辑
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns></returns>
        Task Invoke(HttpContext context);
    }
}