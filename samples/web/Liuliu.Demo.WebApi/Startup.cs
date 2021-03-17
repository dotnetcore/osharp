// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-02 14:35</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Liuliu.Demo.Web.Startups;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Routing;
using OSharp.AutoMapper;
using OSharp.Collections;
using OSharp.Hosting.Authorization;
using OSharp.Hosting.Identity;
using OSharp.Hosting.Infos;
using OSharp.Hosting.Systems;
using OSharp.Log4Net;
using OSharp.MiniProfiler;
using OSharp.Redis;
using OSharp.Swagger;


namespace Liuliu.Demo.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
#if NET5_0
            services.AddDatabaseDeveloperPageExceptionFilter();
#endif
            services.AddOSharp().AddPack<Log4NetPack>()
                .AddPack<AutoMapperPack>()
                .AddPack<EndpointsPack>()
                .AddPack<MiniProfilerPack>()
                .AddPack<SwaggerPack>()
                .AddPack<RedisPack>()
                .AddPack<SystemsPack>()
                .AddPack<AuthenticationPack>()
                .AddPack<FunctionAuthorizationPack>()
                .AddPack<DataAuthorizationPack>()
                .AddPack<SqliteDefaultDbContextMigrationPack>()
                .AddPack<AuditPack>()
                .AddPack<InfosPack>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
#if NET5_0
                app.UseMigrationsEndPoint();
#else
                app.UseDatabaseErrorPage();
#endif
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseMiddleware<JsonExceptionHandlerMiddleware>()
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseOSharp();
        }
    }
}
