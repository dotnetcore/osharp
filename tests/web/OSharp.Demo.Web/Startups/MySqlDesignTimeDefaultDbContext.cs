using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSharp.Dependency;
using OSharp.Entity;

namespace OSharp.Demo.Web
{
    [IgnoreDependency]
    public class MySqlDesignTimeDefaultDbContext : DefaultDbContext
    {
        public MySqlDesignTimeDefaultDbContext(DbContextOptions options, IEntityConfigurationTypeFinder typeFinder)
            : base(options, typeFinder)
        {
        }
    }
}
