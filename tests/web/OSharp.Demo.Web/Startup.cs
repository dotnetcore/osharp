// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-12 0:01</last-date>
// -----------------------------------------------------------------------

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.Mvc.ModelBinding;
using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Entities;


namespace OSharp.Demo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOSharp();
            services.AddOSharpIdentity<UserStore, RoleStore, User, Role, int, int>();

            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new StringTrimModelBinderProvider());
                options.Filters.Add<UnitOfWorkAttribute>();
            });
            services.AddLogging(builder =>
            {
                builder.AddFile(ops =>
                {
                    ops.FileName = "log-";
                    ops.LogDirectory = "logs";
                });
            });

            services.AddDistributedMemoryCache();
            services.AddDistributedRedisCache(ops =>
            {
                ops.Configuration = "127.0.0.1:6379";
                ops.InstanceName = "osharp.demo.web";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            env.EnvironmentName = "Development";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseOSharp().UseAutoMapper();
        }
    }
}