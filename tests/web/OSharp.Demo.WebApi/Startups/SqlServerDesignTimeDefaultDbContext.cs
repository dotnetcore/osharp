using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSharp.Dependency;
using OSharp.Entity;

namespace OSharp.Demo.WebApi
{
    [IgnoreDependency]
    public class SqlServerDesignTimeDefaultDbContext : DefaultDbContext
    {
        public SqlServerDesignTimeDefaultDbContext(DbContextOptions options, IEntityConfigurationTypeFinder typeFinder)
            : base(options, typeFinder)
        {
        }
    }
}
