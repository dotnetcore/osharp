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


namespace Liuliu.Demo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var startup = new Startup();
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();
            startup.Configure(app);
            app.Run();
        }

    }
}