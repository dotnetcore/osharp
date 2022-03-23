// -----------------------------------------------------------------------
//  <copyright file="Startup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-08-26 10:26</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using OSharp.AutoMapper;
using OSharp.CodeGeneration.Generates;
using OSharp.CodeGeneration.Services;
using OSharp.CodeGenerator.Data;
using OSharp.CodeGenerator.Views;
using OSharp.Log4Net;
using OSharp.Reflection;


namespace OSharp.CodeGenerator
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            string[] filters = { "dotnet-", "Microsoft.", "mscorlib", "netstandard", "System", "Windows", "PropertyChanged" };
            AssemblyManager.AssemblyFilterFunc = name => name.Name != null && !filters.Any(m => name.Name.StartsWith(m));

            services.AddOSharp()
                .AddPack<ViewModelPack>()
                .AddPack<Log4NetPack>()
                .AddPack<AutoMapperPack>()
                .AddPack<SqliteDefaultDbContextMigrationPack>()
                .AddPack<DataPack>()
                .AddPack<GeneratePack>();
        }

        public void Configure(IServiceProvider provider)
        {
            provider.UseOsharp();
        }
    }
}
