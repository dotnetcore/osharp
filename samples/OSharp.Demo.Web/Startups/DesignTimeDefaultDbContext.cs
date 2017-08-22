using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSharp.Entity;

namespace OSharp.Demo.Web
{
    [OSharp.Dependency.IgnoreDependency]
    public class DesignTimeDefaultDbContext : DefaultDbContext
    {
        public DesignTimeDefaultDbContext(DbContextOptions options, IEntityConfigurationTypeFinder typeFinder)
            : base(options, typeFinder)
        {
        }
    }
}
