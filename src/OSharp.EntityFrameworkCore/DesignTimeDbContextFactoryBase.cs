// -----------------------------------------------------------------------
//  <copyright file="DesignTimeDbContextFactoryBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-20 17:10</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace OSharp.Entity
{
    /// <summary>
    /// 设计时数据上下文实例工厂基类，用于执行数据迁移
    /// </summary>
    public abstract class DesignTimeDbContextFactoryBase<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// 创建一个数据上下文实例
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public virtual TDbContext CreateDbContext(string[] args)
        {
            string connString = GetConnectionString();
            IEntityConfigurationTypeFinder typeFinder = GetEntityConfigurationTypeFinder();
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<TDbContext>();
            builder = UseSql(builder, connString);
            return (TDbContext)Activator.CreateInstance(typeof(TDbContext), builder.Options, typeFinder);
        }

        /// <summary>
        /// 重写以获取数据上下文数据库连接字符串
        /// </summary>
        public abstract string GetConnectionString();

        /// <summary>
        /// 重写以获取数据实体类配置类型查找器
        /// </summary>
        /// <returns></returns>
        public abstract IEntityConfigurationTypeFinder GetEntityConfigurationTypeFinder();

        /// <summary>
        /// 重写以实现数据上下文选项构建器加载数据库驱动程序
        /// </summary>
        /// <param name="builder">数据上下文选项构建器</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <returns>数据上下文选项构建器</returns>
        public abstract DbContextOptionsBuilder UseSql(DbContextOptionsBuilder builder, string connString);
    }
}