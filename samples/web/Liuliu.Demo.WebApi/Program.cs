// -----------------------------------------------------------------------
//  <copyright file="Program.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-02 11:32</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace Liuliu.Demo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if NET6_0_OR_GREATER
            CreateApplication(args).Run();
#else
            CreateHostBuilder(args).Build().Run();
#endif
        }

#if NET6_0_OR_GREATER

        public static WebApplication CreateApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Startup startup = new Startup();
            startup.ConfigureServices(builder.Services);
            var app = builder.Build();
            startup.Configure(app, app.Environment);
            return app;
        }
#else
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

#endif
    }
}
