// -----------------------------------------------------------------------
//  <copyright file="Program.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-02 11:32</last-date>
// -----------------------------------------------------------------------

#if NET6_0_OR_GREATER

using Liuliu.Demo.Web;

var builder = WebApplication.CreateBuilder(args);

Startup startup = new Startup();
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app);
app.Run();

#else

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace Liuliu.Demo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
#endif