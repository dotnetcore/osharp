// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-26 21:47</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Authorization;
using Liuliu.Demo.Identity;
using Liuliu.Demo.Infos;
using Liuliu.Demo.Systems;
using Liuliu.Demo.Web.Startups;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Routing;
using OSharp.AutoMapper;
using OSharp.Core;
using OSharp.Log4Net;
//using OSharp.MiniProfiler;
using OSharp.Swagger;


namespace Liuliu.Demo.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor(); // 注册 IHttpContextAccessor

            services.AddScoped<Tenant>(provider =>
            {
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var httpContext = httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    return GetTenantByDomain(httpContext.Request.Host.Value);
                }
                return new Tenant
                {
                    Id = 1,
                    Name = "Default Tenant",
                    Domain = "default",
                    ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=osharpns-dev01;Trusted_Connection=True;MultipleActiveResultSets=true"
                };
            });

            //services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddOSharp()
                .AddPack<Log4NetPack>()
                .AddPack<AutoMapperPack>()
                .AddPack<EndpointsPack>()
                //.AddPack<MiniProfilerPack>()
                .AddPack<SwaggerPack>()
                //.AddPack<RedisPack>()
                .AddPack<AuthenticationPack>()
                .AddPack<FunctionAuthorizationPack>()
                .AddPack<DataAuthorizationPack>()
                .AddPack<SqlServerDefaultDbContextMigrationPack>()
                .AddPack<AuditPack>()
                .AddPack<InfosPack>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/#/500");
                app.UseHsts();
                //app.UseHttpsRedirection(); // 启用HTTPS
            }

            //app.UseMiddleware<HostHttpCryptoMiddleware>();
            //app.UseMiddleware<JsonNoFoundHandlerMiddleware>();
            app.UseMiddleware<JsonExceptionHandlerMiddleware>();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseOSharp();
        }

        private Tenant GetTenantByDomain(string domain)
        {
            // 根据域名获取租户信息的逻辑
            // 这里可以从数据库或配置文件中获取
            return new Tenant
            {
                Id = 1,
                Name = "Default Tenant",
                Domain = domain,
                ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=osharpns-dev02;Trusted_Connection=True;MultipleActiveResultSets=true"
            };
        }
    }
}