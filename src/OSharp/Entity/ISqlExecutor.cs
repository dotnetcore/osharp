// -----------------------------------------------------------------------
//  <copyright file="ISqlQuery.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-15 18:22</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using OSharp.Dependency;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义SQL语句执行功能
    /// </summary>
    [MultipleDependency]
    public interface ISqlExecutor<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// 获取 数据库类型
        /// </summary>
        DatabaseType DatabaseType { get; }

        /// <summary>
        /// 查询指定SQL的结果集
        /// </summary>
        /// <typeparam name="TResult">结果集类型</typeparam>
        /// <param name="sql">查询的SQL语句</param>
        /// <param name="param">SQL参数</param>
        /// <returns>结果集</returns>
        IEnumerable<TResult> FromSql<TResult>(string sql, object param = null);

        /// <summary>
        /// 执行指定的SQL语句
        /// </summary>
        /// <param name="sql">执行的SQL语句</param>
        /// <param name="param">SQL参数</param>
        /// <returns>操作影响的行数</returns>
        int ExecuteSqlCommand(string sql, object param = null);
    }
}