// -----------------------------------------------------------------------
//  <copyright file="Program.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using AspectCore.Extensions.Hosting;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OSharp.Core;
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
using OSharp.Log4Net;
using OSharp.MiniProfiler;
using OSharp.Swagger;
using Microsoft.AspNetCore.Http;
using OSharp.Entity;
using OSharp.MultiTenancy;
using OSharp.Entity.SqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FileTenantStoreOptions>(options =>
{
    options.FilePath = "App_Data/tenants.json";
});

// 添加服务到容器
// 注册租户访问器
builder.Services.AddScoped<ITenantAccessor, AsyncLocalTenantAccessor>();
//builder.Services.AddScoped<ITenantAccessor, TenantAccessor>();
builder.Services.AddScoped<HttpTenantProvider>();

// 注册租户存储
//builder.Services.AddSingleton<ITenantStore, ConfigurationTenantStore>();
builder.Services.AddSingleton<ITenantStore, FileTenantStore>();

builder.Services.AddHttpContextAccessor(); // 注册 IHttpContextAccessor

// 注册租户数据库迁移器
builder.Services.AddSingleton<TenantDatabaseMigrator>();

// 替换默认的连接字符串提供者
//builder.Services.AddScoped<IConnectionStringProvider, MultiTenantConnectionStringProvider>();
builder.Services.Replace<IConnectionStringProvider, MultiTenantConnectionStringProvider>(ServiceLifetime.Scoped);

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddOSharp()
    .AddPack<Log4NetPack>()
    .AddPack<AutoMapperPack>()
    .AddPack<EndpointsPack>()
    .AddPack<MiniProfilerPack>()
    .AddPack<SwaggerPack>()
    //.AddPack<RedisPack>()
    .AddPack<AuthenticationPack>()
    .AddPack<FunctionAuthorizationPack>()
    .AddPack<DataAuthorizationPack>()
    .AddPack<SqlServerDefaultDbContextMigrationPack>()
    .AddPack<AuditPack>()
    .AddPack<InfosPack>();

var app = builder.Build();

// 在请求管道中注册 TenantMiddleware
// 注意：应该在 UseRouting 之后，但在 UseAuthentication 和 UseAuthorization 之前注册
app.UseRouting();

// 添加多租户中间件
app.UseMiddleware<TenantMiddleware>();

// 使用 OSharp
app.UseOSharp();

using (var scope = app.Services.CreateScope())
{
    var migrator = scope.ServiceProvider.GetRequiredService<TenantDatabaseMigrator>();
    await migrator.MigrateAllTenantsAsync();
}

app.Run();