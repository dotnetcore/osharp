// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.AspNetCore;
using OSharp.Data;


namespace Liuliu.Demo.Web
{
    public class Startup 
    { 
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Singleton<IConfiguration>.Instance = configuration;
            Singleton<IHostingEnvironment>.Instance = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOSharp<AspOsharpPackManager>();
        }
         
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/#/500");
                app.UseHsts().UseHttpsRedirection();
            }

            app.UseMiddleware<NodeNoFoundHandlerMiddleware>()
                .UseMiddleware<NodeExceptionHandlerMiddleware>()
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseOSharp();
        }
    }
}