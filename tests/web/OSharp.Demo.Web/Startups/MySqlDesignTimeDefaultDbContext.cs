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
        /// <summary>
        /// 初始化一个<see cref="DefaultDbContext"/>类型的新实例
        /// </summary>
        public MySqlDesignTimeDefaultDbContext(DbContextOptions options, IEntityConfigurationTypeFinder typeFinder)
            : base(options, typeFinder)
        { }
    }
}
