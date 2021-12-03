﻿// -----------------------------------------------------------------------
//  <copyright file="MySqlDapperSqlExecutor.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-08-21 13:37</last-date>
// -----------------------------------------------------------------------

using System;
using System.Data;

#if NET5_0
using MySqlConnector;
#else
using MySql.Data.MySqlClient;
#endif

namespace OSharp.Entity.MySql
{
    /// <summary>
    /// MySql的Dapper-Sql功能执行
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">编号类型</typeparam>
    public class MySqlDapperSqlExecutor<TEntity, TKey> : SqlExecutorBase<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// 初始化一个<see cref="SqlExecutorBase{TEntity,TKey}"/>类型的新实例
        /// </summary>
        public MySqlDapperSqlExecutor(IServiceProvider provider)
            : base(provider)
        { }

        /// <summary>
        /// 获取 数据库类型
        /// </summary>
        public override DatabaseType DatabaseType => DatabaseType.MySql;

        /// <summary>
        /// 重写以获取数据连接对象
        /// </summary>
        /// <param name="connectionString">数据连接字符串</param>
        /// <returns></returns>
        protected override IDbConnection GetDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}