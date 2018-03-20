using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Entity;
using OSharp.Exceptions;
using OSharp.Reflection;

namespace OSharp.Demo.WebApi.Startups
{
    public class SqlServerDesignTimeDefaultDbContextFactory : DesignTimeDbContextFactoryBase<DefaultDbContext>
    {
        private readonly IServiceProvider _serviceProvider;

        public SqlServerDesignTimeDefaultDbContextFactory()
        { }

        public SqlServerDesignTimeDefaultDbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override string GetConnectionString()
        {
            if (_serviceProvider == null)
            {
                string str = AppSettingsManager.Get("OSharp:DbContexts:SqlServer:ConnectionString");
                if (str == null)
                {
                    str = AppSettingsManager.Get("ConnectionStrings:DefaultDbContext");
                }
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
            return _serviceProvider != null
                ? _serviceProvider.GetService<IEntityConfigurationTypeFinder>()
                : new EntityConfigurationTypeFinder(new EntityConfigurationAssemblyFinder(new AppDomainAllAssemblyFinder()));
        }

        public override DbContextOptionsBuilder UseSql(DbContextOptionsBuilder builder, string connString)
        {
            string entryAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            Console.WriteLine($"entryAssemblyName: {entryAssemblyName}");
            return builder.UseSqlServer(connString, b => b.MigrationsAssembly(entryAssemblyName));
        }
    }
}
