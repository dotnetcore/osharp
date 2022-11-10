// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-27 23:41</last-date>
// -----------------------------------------------------------------------


namespace OSharp.Authorization;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加名称为 OsharpPolicy 的授权策略
    /// </summary>
    public static IServiceCollection AddFunctionAuthorizationHandler(this IServiceCollection services)
    {
        //OsharpOptions options = services.GetOsharpOptions();

        //services.AddAuthorization();
        services.AddAuthorization(opts =>
        {
            opts.AddPolicy(FunctionRequirement.OsharpPolicy, policy =>
            {
                policy.Requirements.Add(new FunctionRequirement());
                //policy.AuthenticationSchemes.AddIf(JwtBearerDefaults.AuthenticationScheme, options.Jwt?.Enabled == true);
                //policy.AuthenticationSchemes.AddIf(CookieAuthenticationDefaults.AuthenticationScheme, options.Cookie?.Enabled == true);
            });
        });
        services.AddSingleton<IAuthorizationHandler, FunctionAuthorizationHandler>();

        return services;
    }

    /// <summary>
    /// 应用功能权限授权
    /// </summary>
    public static IApplicationBuilder UseFunctionAuthorization(this IApplicationBuilder app)
    {
        app.UseAuthorization();

        IServiceProvider provider = app.ApplicationServices;

        IModuleHandler moduleHandler = provider.GetRequiredService<IModuleHandler>();
        moduleHandler.Initialize();

        IFunctionHandler functionHandler = provider.GetRequiredService<IFunctionHandler>();
        functionHandler.RefreshCache();

        IFunctionAuthCache functionAuthCache = provider.GetRequiredService<IFunctionAuthCache>();
        functionAuthCache.BuildRoleCaches();

        return app;
    }
}
