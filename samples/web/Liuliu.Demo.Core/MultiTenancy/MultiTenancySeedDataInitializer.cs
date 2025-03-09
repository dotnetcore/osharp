// -----------------------------------------------------------------------
//  <copyright file="ModuleSeedDataInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-07 1:01</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq.Expressions;

using Liuliu.Demo.Authorization.Entities;
using Liuliu.Demo.MultiTenancy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Core.Options;
using OSharp.Data.Snows;
using OSharp.Entity;


namespace Liuliu.Demo.MultiTenancy
{
    public class MultiTenancySeedDataInitializer : SeedDataInitializerBase<Tenant, long>
    {
        /// <summary>
        /// 初始化一个<see cref="MultiTenancySeedDataInitializer"/>类型的新实例
        /// </summary>
        public MultiTenancySeedDataInitializer(IServiceProvider rootProvider)
            : base(rootProvider)
        { }

        /// <summary>
        /// 重写以提供要初始化的种子数据
        /// </summary>
        /// <returns></returns>
        protected override Tenant[] SeedData(IServiceProvider provider)
        {
            long id = 10000;

            var _dbContexts = provider.GetOSharpOptions().DbContexts;
            OsharpDbContextOptions dbContextOptions = _dbContexts.Values.FirstOrDefault(m => m.DbContextType == typeof(DefaultDbContext));
            var connectionString = dbContextOptions.ConnectionString;

            return new[]
            {
                new Tenant() {Id = id, Name = "杭州智密科技有限公司",  ShortName="智密科技", ConnectionString=connectionString, Host="localhost", IsEnabled = true, TenantKey="Default" },
            };
        }

        /// <summary>
        /// 重写以提供判断某个实体是否存在的表达式
        /// </summary>
        /// <param name="entity">要判断的实体</param>
        /// <returns></returns>
        protected override Expression<Func<Tenant, bool>> ExistingExpression(Tenant entity)
        {
            return m => m.TenantKey == entity.TenantKey;
        }
    }
}