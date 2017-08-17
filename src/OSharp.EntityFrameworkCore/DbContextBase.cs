// -----------------------------------------------------------------------
//  <copyright file="DbContextBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-16 22:10</last-date>
// -----------------------------------------------------------------------

using System;
using System.Data;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace OSharp.Entity
{
    /// <summary>
    /// EntityFramework上下文基类
    /// </summary>
    public abstract class DbContextBase<TDbContext> : DbContext, IUnitOfWork
    {
        private readonly IEntityConfigurationTypeFinder _typeFinder;

        /// <summary>
        /// 初始化一个<see cref="DbContextBase{TDbContext}"/>类型的新实例
        /// </summary>
        protected DbContextBase(DbContextOptions options, IEntityConfigurationTypeFinder typeFinder)
            : base(options)
        {
            _typeFinder = typeFinder;
        }

        /// <summary>
        /// 获取 是否开启事务提交
        /// </summary>
        public bool TransactionEnabled => Database.CurrentTransaction != null;

        /// <summary>
        /// 显式开启数据上下文事务
        /// </summary>
        public void BeginTransaction()
        {
            if (Database.CurrentTransaction == null)
            {
                Database.BeginTransaction();
            }
        }

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        public void Commit()
        {
            IDbContextTransaction trans = Database.CurrentTransaction;
            if (trans != null)
            {
                try
                {
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 显式回滚事务，仅在显式开启事务后有用
        /// </summary>
        public void Rollback()
        {
            IDbContextTransaction trans = Database.CurrentTransaction;
            if (trans != null)
            {
                trans.Rollback();
            }
        }

        /// <summary>
        /// 创建上下文数据模型时，对各个实体类的数据库映射细节进行配置
        /// </summary>
        /// <param name="modelBuilder">上下文数据模型构建器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //通过实体配置信息将实体注册到当前上下文
            IEntityRegister[] registers = _typeFinder.GetEntityRegisters(typeof(TDbContext));
            foreach (IEntityRegister register in registers)
            {
                register.RegistTo(modelBuilder);
            }
        }
    }
}