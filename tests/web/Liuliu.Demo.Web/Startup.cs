// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;

using Liuliu.Demo.Web.Mappers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json.Serialization;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Conventions;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Core;

using Swashbuckle.AspNetCore.Swagger;


namespace Liuliu.Demo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment())
            {
                services.AddMvcCore().AddApiExplorer();
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info() { Title = "OSharpNS API", Version = "v1" });
                    Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                    {
                        options.IncludeXmlComments(file);
                    });
                });
            }

            services.AddMvc(options =>
            {
                options.Conventions.Add(new DashedRoutingConvention());
                options.Filters.Add(new FunctionAuthorizationFilter()); //全局功能权限过滤器
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddOSharp().AddDistributedMemoryCache()
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
                string secret = Configuration["OSharp:Jwt:Secret"] ?? Configuration["JwtSecret"];
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["OSharp:Jwt:Issuer"],
                    ValidAudience = Configuration["OSharp:Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret))
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
                app.UseSwagger().UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "OSharpNS API V1");
                });
            }
            else
            {
                app.UseExceptionHandler("/#/500");
                app.UseHsts().UseHttpsRedirection();
            }

            app.UseMiddleware<NodeNoFoundHandlerMiddleware>()
                .UseMiddleware<NodeExceptionHandlerMiddleware>()
                .UseDtoMappings()
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseAuthentication()
                .UseMvcWithAreaRoute()
                .UseSignalR(opts =>
                {
                    //opts.MapHub<>();
                })
                .UseOSharp();
        }
    }
}