// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-19 18:13</last-date>
// -----------------------------------------------------------------------

using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json.Serialization;

using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Conventions;
using OSharp.AspNetCore.Mvc.Filters;

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
                options.Conventions.Add(new DashedRoutingConvention());
                options.Filters.Add(new FunctionAuthorizationFilter()); //全局功能权限过滤器
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["OSharp:Jwt:Issuer"],
                    ValidAudience = Configuration["OSharp:Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtSecret"]))
                };
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler();
                app.UseHsts();
            }

            app.UseMiddleware<NodeNoFoundHandlerMiddleware>()
                .UseMiddleware<NodeExceptionHandlerMiddleware>()
                //.UseHttpsRedirection()
                .UseDefaultFiles().UseStaticFiles()
                .UseAuthentication()
                .UseMvcWithAreaRoute()
                .UseSignalR(cfg =>
                    { })
                .UseOSharp();
        }
    }
}