// -----------------------------------------------------------------------
//  <copyright file="SqlServerDesignTimeDefaultDbContextFactory.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-08-16 14:15</last-date>
// -----------------------------------------------------------------------

using System;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

using OSharp.Entity;


namespace Liuliu.Demo.Web.Startups
{
    public class SqlServerDesignTimeDefaultDbContextFactory : DesignTimeDbContextFactoryBase<DefaultDbContext>
    {
        public SqlServerDesignTimeDefaultDbContextFactory()
            : base(null)
        { }

        public SqlServerDesignTimeDefaultDbContextFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override DbContextOptionsBuilder UseSql(DbContextOptionsBuilder builder, string connString)
        {
            string entryAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            Console.WriteLine($"entryAssemblyName: {entryAssemblyName}");
            return builder.UseSqlServer(connString, b => b.MigrationsAssembly(entryAssemblyName));
        }
    }
}
