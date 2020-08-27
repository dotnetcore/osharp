// -----------------------------------------------------------------------
//  <copyright file="SqlServerDefaultDbContextMigrationPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using OSharp.Core.Packs;
using OSharp.Entity;
using OSharp.Entity.SqlServer;


namespace Liuliu.Demo.Web.Startups
{
    /// <summary>
    /// SqlServer-DefaultDbContext迁移模块
    /// </summary>
    [DependsOnPacks(typeof(SqlServerEntityFrameworkCorePack))]
    [Description("SqlServer-DefaultDbContext迁移模块")]
    public class SqlServerDefaultDbContextMigrationPack : MigrationPackBase<DefaultDbContext>
    {
        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 2;

        /// <summary>
        /// 获取 数据库类型
        /// </summary>
        protected override DatabaseType DatabaseType => DatabaseType.SqlServer;

        protected override DefaultDbContext CreateDbContext(IServiceProvider scopedProvider)
        {
            return new DesignTimeDefaultDbContextFactory(scopedProvider).CreateDbContext(new string[0]);
        }
    }
}
