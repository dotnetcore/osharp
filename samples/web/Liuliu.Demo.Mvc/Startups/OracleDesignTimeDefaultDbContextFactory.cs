// -----------------------------------------------------------------------
//  <copyright file="OracleDesignTimeDefaultDbContextFactory.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-04-18 21:00</last-date>
// -----------------------------------------------------------------------

using System;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Reflection;


namespace Liuliu.Demo.Web.Startups
{
    public class OracleDesignTimeDefaultDbContextFactory : DesignTimeDbContextFactoryBase<DefaultDbContext>
    {
        private readonly IServiceProvider _serviceProvider;

        public OracleDesignTimeDefaultDbContextFactory()
        { }

        public OracleDesignTimeDefaultDbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override string GetConnectionString()
        {
            if (_serviceProvider == null)
            {
                IConfiguration configuration = Singleton<IConfiguration>.Instance;
                string str = configuration["OSharp:DbContexts:Oracle:ConnectionString"];
                return str;
            }
            OsharpOptions options = _serviceProvider.GetOSharpOptions();
            OsharpDbContextOptions contextOptions = options.GetDbContextOptions(typeof(DefaultDbContext));
            if (contextOptions == null)
            {
                throw new OsharpException($"上下文“{typeof(DefaultDbContext)}”的配置信息不存在");
            }
            return contextOptions.ConnectionString;
        }

        public override IEntityManager GetEntityManager()
        {
            if (_serviceProvider != null)
            {
                return _serviceProvider.GetService<IEntityManager>();
            }
            IEntityConfigurationTypeFinder typeFinder = new EntityConfigurationTypeFinder(new AppDomainAllAssemblyFinder());
            IEntityManager entityManager = new EntityManager(typeFinder);
            entityManager.Initialize();
            return entityManager;
        }

        public override bool LazyLoadingProxiesEnabled()
        {
            if (_serviceProvider == null)
            {
                IConfiguration configuration = Singleton<IConfiguration>.Instance;
                return configuration["OSharp:DbContexts:Oracle:LazyLoadingProxiesEnabled"].CastTo(false);
            }
            OsharpOptions options = _serviceProvider.GetOSharpOptions();
            OsharpDbContextOptions contextOptions = options.GetDbContextOptions(typeof(DefaultDbContext));
            if (contextOptions == null)
            {
                throw new OsharpException($"上下文“{typeof(DefaultDbContext)}”的配置信息不存在");
            }

            return contextOptions.LazyLoadingProxiesEnabled;
        }

        public override DbContextOptionsBuilder UseSql(DbContextOptionsBuilder builder, string connString)
        {
            string entryAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            Console.WriteLine($"entryAssemblyName: {entryAssemblyName}");
            return builder.UseOracle(connString, b => b.MigrationsAssembly(entryAssemblyName));
        }
    }
}