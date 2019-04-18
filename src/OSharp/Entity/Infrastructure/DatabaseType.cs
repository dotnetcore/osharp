// -----------------------------------------------------------------------
//  <copyright file="DatabaseType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-21 1:11</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Entity
{
    /// <summary>
    /// 表示数据库类型，如SqlServer，Sqlite等
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// SqlServer数据库类型
        /// </summary>
        SqlServer,
        /// <summary>
        /// Sqlite数据库类型
        /// </summary>
        Sqlite,
        /// <summary>
        /// MySql数据库类型
        /// </summary>
        MySql,
        /// <summary>
        /// PostgreSql数据库类型
        /// </summary>
        PostgreSql,
        /// <summary>
        /// Oracle数据库类型
        /// </summary>
        Oracle
    }
}