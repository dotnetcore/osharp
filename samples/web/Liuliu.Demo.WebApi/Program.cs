// -----------------------------------------------------------------------
//  <copyright file="Program.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-10 13:47</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Web;

var builder = WebApplication.CreateBuilder(args);

Startup startup = new Startup();
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app);
app.Run();