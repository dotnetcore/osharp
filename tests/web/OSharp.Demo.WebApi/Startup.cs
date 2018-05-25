// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-08 19:55</last-date>
// -----------------------------------------------------------------------

using System.Security.Claims;
using System.Spatial;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json.Serialization;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Conventions;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Audits;
using OSharp.AutoMapper;
using OSharp.Core.EntityInfos;
using OSharp.Core.Options;
using OSharp.Demo.Identity;
using OSharp.Demo.Security;
using OSharp.Entity;
using OSharp.Security.JwtBearer;


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
                options.Filters.Add(new FunctionAuthorizationFilter());//全局功能权限过滤器
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
            }

            app.UseMiddleware<NodeNoFoundHandlerMiddleware>()
                .UseMiddleware<NodeExceptionHandlerMiddleware>()
                .UseDefaultFiles().UseStaticFiles()
                .UseAuthentication()
                .UseMvcWithAreaRoute().UseOSharp();
        }
    }
}