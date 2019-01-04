using System;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Entity;
using OSharp.Exceptions;
using OSharp.Reflection;


namespace Liuliu.Demo.Web.Startups
{
    public class PostgreSqlDesignTimeDefaultDbContextFactory : DesignTimeDbContextFactoryBase<DefaultDbContext>
    {
        private readonly IServiceProvider _serviceProvider;

        public PostgreSqlDesignTimeDefaultDbContextFactory()
        { }

        public PostgreSqlDesignTimeDefaultDbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override string GetConnectionString()
        {
            if (_serviceProvider == null)
            {
                string str = AppSettingsManager.Get("OSharp:DbContexts:PostgreSql:ConnectionString");
                return str;
            }
            OSharpOptions options = _serviceProvider.GetOSharpOptions();
            OSharpDbContextOptions contextOptions = options.GetDbContextOptions(typeof(DefaultDbContext));
            if (contextOptions == null)
            {
                throw new OsharpException($"上下文“{typeof(DefaultDbContext)}”的配置信息不存在");
            }
            return contextOptions.ConnectionString;
        }

        public override IEntityConfigurationTypeFinder GetEntityConfigurationTypeFinder()
        {
            if (_serviceProvider != null)
            {
                return _serviceProvider.GetService<IEntityConfigurationTypeFinder>();
            }
            IEntityConfigurationTypeFinder typeFinder = new EntityConfigurationTypeFinder(new AppDomainAllAssemblyFinder());
            typeFinder.Initialize();
            return typeFinder;
        }

        public override DbContextOptionsBuilder UseSql(DbContextOptionsBuilder builder, string connString)
        {
            string entryAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            Console.WriteLine($"entryAssemblyName: {entryAssemblyName}");
            return builder.UseNpgsql(connString, b => b.MigrationsAssembly(entryAssemblyName));
        }
    }
}
