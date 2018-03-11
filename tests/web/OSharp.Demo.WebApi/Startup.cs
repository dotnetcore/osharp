// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-08 19:55</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Serialization;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Audits;
using OSharp.AutoMapper;
using OSharp.Demo.Identity;
using OSharp.Demo.WebApi.Controllers;
using OSharp.Entity;
using OSharp.Entity.SqlServer;


namespace OSharp.Demo.WebApi
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
            services.AddMvc(options =>
            {
                options.Filters.Add<UnitOfWorkAttribute>();
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddOSharp();

            services.AddDistributedMemoryCache()
                .AddLogging(builder =>
                {
                    builder.AddFile(options =>
                    {
                        options.FileName = "log-";
                        options.LogDirectory = "log";
                    });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseMvcWithAreaRoute().UseOSharpMvc();

            //数据库迁移
            SqlServerDesignTimeDefaultDbContextFactory dbContextFactory = new SqlServerDesignTimeDefaultDbContextFactory();
            var dbContext = dbContextFactory.CreateDbContext(new string[0]);
            dbContext.CheckAndMigration();
        }
    }
}