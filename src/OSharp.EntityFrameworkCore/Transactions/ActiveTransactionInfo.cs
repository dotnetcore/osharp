// -----------------------------------------------------------------------
//  <copyright file="ActiveTransactionInfo.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-19 13:24</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


//设计参考：ABP https://github.com/aspnetboilerplate/aspnetboilerplate
namespace OSharp.Entity
{
    /// <summary>
    /// 活动的事务信息
    /// </summary>
    public class ActiveTransactionInfo
    {
        /// <summary>
        /// 初始化一个<see cref="ActiveTransactionInfo"/>类型的新实例
        /// </summary>
        /// <param name="dbContextTransaction">共享的上下文事务对象</param>
        /// <param name="starterDbContext">起始数据上下文</param>
        public ActiveTransactionInfo(IDbContextTransaction dbContextTransaction, DbContext starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;
            AttendedDbContexts = new List<DbContext>();
        }

        /// <summary>
        /// 获取 共享的数据上下文事务
        /// </summary>
        public IDbContextTransaction DbContextTransaction { get; }

        /// <summary>
        /// 获取 第一个数据上下文，事务对象以此上下文事务为准
        /// </summary>
        public DbContext StarterDbContext { get; }

        /// <summary>
        /// 获取 附加的上下文集合，这些上下文的事务对象以<see cref="StarterDbContext"/>上下文的事务为准
        /// </summary>
        public List<DbContext> AttendedDbContexts { get; }
    }
}