using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using OSharp.Core.Options;
using OSharp.Entity;
using OSharp.Reflection;

namespace OSharp.Demo.WebApi
{
    public class SqlServerDesignTimeDefaultDbContextFactory : IDesignTimeDbContextFactory<DefaultDbContext>
    {
        public DefaultDbContext CreateDbContext(string[] args)
        {
            string connString = AppSettingsManager.Get("OSharp:DbContexts:SqlServer:ConnectionString")
                ?? AppSettingsManager.Get("ConnectionStrings:DefaultDbContext");

            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<DefaultDbContext>();
            string entryAssemblyName = Assembly.GetEntryAssembly().GetName().Name;
            builder.UseSqlServer(connString, b => b.MigrationsAssembly(entryAssemblyName));
            IEntityConfigurationTypeFinder typeFinder = new EntityConfigurationTypeFinder(new EntityConfigurationAssemblyFinder(new AppDomainAllAssemblyFinder()));
            return new DefaultDbContext(builder.Options, typeFinder);
        }
    }
}
