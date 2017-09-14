using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Infrastructure;


namespace OSharp
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/>辅助扩展方法
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// OSharp框架初始化
        /// </summary>
        public static IApplicationBuilder UseOSharp(this IApplicationBuilder app)
        {
            IEntityInfoHandler entityInfoHandler = app.ApplicationServices.GetService<IEntityInfoHandler>();
            entityInfoHandler.Initialize();
            return app;
        }
    }
}
