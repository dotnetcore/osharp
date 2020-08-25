// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-02 23:31</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Web.Startups;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Routing;
using OSharp.AutoMapper;
using OSharp.Hangfire;
using OSharp.Hosting.Authorization;
using OSharp.Hosting.Identity;
using OSharp.Hosting.Systems;
using OSharp.Log4Net;
using OSharp.Redis;
using OSharp.Swagger;


namespace Liuliu.Demo.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOSharp().AddPacks(typeof(HangfirePack), typeof(RedisPack));
            //.AddPack<Log4NetPack>()
            //.AddPack<AutoMapperPack>()
            //.AddPack<EndpointsPack>()
            //.AddPack<SwaggerPack>()
            //.AddPack<AuthenticationPack>()
            //.AddPack<FunctionAuthorizationPack>()
            //.AddPack<DataAuthorizationPack>()
            //.AddPack<SqlServerDefaultDbContextMigrationPack>()
            //.AddPack<AuditPack>()
            //.AddPack<SwaggerPack>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware<JsonExceptionHandlerMiddleware>();
            app.UseStaticFiles();

            app.UseOSharp();
        }
    }
}
