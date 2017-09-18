using System;
using System.Collections.Generic;
using System.Linq;
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
            IServiceProvider serviceProvider = app.ApplicationServices;
            //应用程序级别的服务定位器
            ServiceLocator.Instance.TrySetApplicationServiceProvider(serviceProvider);

            IEntityInfoHandler entityInfoHandler = serviceProvider.GetService<IEntityInfoHandler>();
            entityInfoHandler.Initialize();

            IFunctionHandler[] functionHandlers = serviceProvider.GetServices<IFunctionHandler>().ToArray();
            foreach (IFunctionHandler functionHandler in functionHandlers)
            {
                functionHandler.Initialize();
            }

            return app;
        }
    }
}
