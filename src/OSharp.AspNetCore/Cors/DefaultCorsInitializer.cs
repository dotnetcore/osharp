// -----------------------------------------------------------------------
//  <copyright file="DefaultCorsInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-12-13 13:18</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Exceptions;

using CorsOptions = OSharp.Core.Options.CorsOptions;


namespace OSharp.AspNetCore.Cors
{
    /// <summary>
    /// 默认Cors初始化器
    /// </summary>
    public class DefaultCorsInitializer : ICorsInitializer
    {
        private CorsOptions _cors;

        /// <summary>
        /// 添加Cors
        /// </summary>
        public virtual IServiceCollection AddCors(IServiceCollection services)
        {
            IConfiguration configuration = services.GetConfiguration();
            CorsOptions cors = new CorsOptions();
            configuration.Bind("OSharp:Cors", cors);
            _cors = cors;
            if (!cors.Enabled)
            {
                return services;
            }

            if (string.IsNullOrEmpty(cors.PolicyName))
            {
                throw new OsharpException("配置文件中OSharp:Cors节点的PolicyName不能为空");
            }

            services.AddCors(opts => opts.AddPolicy(cors.PolicyName,
                policy =>
                {
                    if (cors.AllowAnyHeader)
                        policy.AllowAnyHeader();
                    else if (cors.WithHeaders != null)
                        policy.WithHeaders(cors.WithHeaders);

                    if (cors.AllowAnyMethod)
                        policy.AllowAnyMethod();
                    else if (cors.WithMethods != null)
                        policy.WithMethods(cors.WithMethods);

                    if (cors.AllowCredentials) policy.AllowCredentials();
                    else if (cors.DisallowCredentials) policy.DisallowCredentials();

                    if (cors.AllowAnyOrigin) policy.AllowAnyOrigin();
                    else if (cors.WithOrigins != null) policy.WithOrigins(cors.WithOrigins);
                }));

            return services;
        }

        /// <summary>
        /// 应用Cors
        /// </summary>
        public virtual IApplicationBuilder UseCors(IApplicationBuilder app)
        {
            if (_cors.Enabled)
            {
                app.UseCors(_cors.PolicyName);
            }

            return app;
        }
    }
}