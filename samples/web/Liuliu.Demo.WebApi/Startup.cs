// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-10 13:46</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Web.Startups;
using Microsoft.Extensions.Caching.Distributed;
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
using OSharp.Caching;
using Liuliu.Demo.Web.Startups.Yitter;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using OSharp.Core.Options;
using System.Linq;

namespace Liuliu.Demo.Web
{
    public class Startup
    {
        public Startup()
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();

            var _configuration = services.GetConfiguration();

            // 添加健康检查服务
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddCheck("sqlite", () =>
                {
                    try
                    {
                        var options = _configuration.GetSection("OSharp").Get<OsharpOptions>();
                        var dbConfig = options?.DbContexts?.FirstOrDefault(m => m.Value.DatabaseType ==  DatabaseType.Sqlite).Value;
                        if (dbConfig == null || string.IsNullOrEmpty(dbConfig.ConnectionString))
                        {
                            return HealthCheckResult.Unhealthy("数据库配置不存在");
                        }
                        using var connection = new Microsoft.Data.Sqlite.SqliteConnection(dbConfig.ConnectionString);
                        connection.Open();
                        return HealthCheckResult.Healthy();
                    }
                    catch (Exception ex)
                    {
                        return HealthCheckResult.Unhealthy(exception: ex);
                    }
                });

            services.AddOSharp()
                .AddPack<Log4NetPack>()
                .AddPack<AutoMapperPack>()
                .AddPack<EndpointsPack>()
                .AddPack<MiniProfilerPack>()
                .AddPack<SwaggerPack>()
                // .AddPack<RedisPack>()
                .AddPack<YitterIdGeneratorPack>()
                .AddPack<SystemsPack>()
                .AddPack<AuthenticationPack>()
                .AddPack<FunctionAuthorizationPack>()
                .AddPack<DataAuthorizationPack>()
                .AddPack<SqliteDefaultDbContextMigrationPack>()
                .AddPack<SqliteTenantDbContextMigrationPack>()
                .AddPack<MultiTenancyPack>()
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
                //app.UseHttpsRedirection();
            }

            // 添加健康检查终端点
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            description = entry.Value.Description,
                            duration = entry.Value.Duration
                        })
                    };
                    await context.Response.WriteAsJsonAsync(result);
                }
            });

            // 添加多租户中间件
            app.UseMiddleware<TenantMiddleware>();

            app //.UseMiddleware<JsonExceptionHandlerMiddleware>()
                .UseDefaultFiles()
                .UseStaticFiles();
            app.UseOSharp();

            //多租户支持代码
            using (var scope = app.Services.CreateScope())
            {
                var key = "MultiTenancy:RunTime:Default";
                var _cache = scope.ServiceProvider.GetService<IDistributedCache>();
                _cache.Set(key, System.DateTime.Now);
            }
        }
    }
}
