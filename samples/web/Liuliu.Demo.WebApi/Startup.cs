// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-10 13:46</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Web.Startups;

using OSharp.AspNetCore.Routing;
using OSharp.AutoMapper;
using OSharp.Entity;
using OSharp.Hangfire;
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
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddOSharp()
                .AddPack<Log4NetPack>()
                .AddPack<AutoMapperPack>()
                .AddPack<EndpointsPack>()
                .AddPack<MiniProfilerPack>()
                .AddPack<SwaggerPack>()
                //.AddPack<RedisPack>()
                .AddPack<SystemsPack>()
                .AddPack<AuthenticationPack>()
                .AddPack<FunctionAuthorizationPack>()
                .AddPack<DataAuthorizationPack>()
                .AddPack<SqlServerDefaultDbContextMigrationPack>()
                //.AddPack<HangfirePack>()
                .AddPack<AuditPack>()
                .AddPack<InfosPack>();
            
            services.AddSingleton<IEntityBatchConfiguration, PropertyCommentConfiguration>();
            services.AddSingleton<IEntityBatchConfiguration, PropertyUtcDateTimeConfiguration>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app)
        {
            IWebHostEnvironment env = app.Environment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app //.UseMiddleware<JsonExceptionHandlerMiddleware>()
                .UseDefaultFiles()
                .UseStaticFiles();
            app.UseOSharp();
        }
    }
}
