using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OSharp.Entity;
using OSharp.Reflection;

namespace OSharp.Demo.Web.Startups
{
    public class SqlServerDesignTimeDefaultDbContextFactory : IDesignTimeDbContextFactory<SqlServerDesignTimeDefaultDbContext>
    {
        public SqlServerDesignTimeDefaultDbContext CreateDbContext(string[] args)
        {
            string connString = "Server=.;Database=osharp.demo.web.ng;Trusted_Connection=True;MultipleActiveResultSets=true";
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<DefaultDbContext>();
            builder.UseSqlServer(connString);
            IEntityConfigurationTypeFinder typeFinder = new EntityConfigurationTypeFinder(new EntityConfigurationAssemblyFinder(new AppDomainAllAssemblyFinder()));
            return new SqlServerDesignTimeDefaultDbContext(builder.Options, typeFinder);
        }
    }
}
