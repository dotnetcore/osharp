// -----------------------------------------------------------------------
//  <copyright file="SqlServerDefaultDbContextMigrationModule.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-21 12:36</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Security;
using OSharp.Demo.Security.Dtos;
using OSharp.Demo.Security.Entities;
using OSharp.Entity;
using OSharp.Entity.SqlServer;


namespace OSharp.Demo.WebApi.Startups
{
    /// <summary>
    /// SqlServer-DefaultDbContext迁移模块
    /// </summary>
    public class SqlServerDefaultDbContextMigrationModule : SqlServerMigrationModuleBase<DefaultDbContext>
    {
        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 2;

        protected override DefaultDbContext CreateDbContext(IServiceProvider scopedProvider)
        {
            return new SqlServerDesignTimeDefaultDbContextFactory(scopedProvider).CreateDbContext(new string[0]);
        }
    }
}