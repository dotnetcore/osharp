// -----------------------------------------------------------------------
//  <copyright file="OracleDapperSqlExecutor.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-11 1:26</last-date>
// -----------------------------------------------------------------------

using Oracle.ManagedDataAccess.Client;


namespace OSharp.Entity.Oracle;

/// <summary>
/// Oracle 的Dapper-Sql功能执行
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">编号类型</typeparam>
public class OracleDapperSqlExecutor<TEntity, TKey> : SqlExecutorBase<TEntity, TKey> where TEntity : IEntity<TKey>
{
    /// <summary>
    /// 初始化一个<see cref="SqlExecutorBase{TEntity,TKey}"/>类型的新实例
    /// </summary>
    public OracleDapperSqlExecutor(IServiceProvider provider)
        : base(provider)
    { }

    /// <summary>
    /// 获取 数据库类型
    /// </summary>
    public override DatabaseType DatabaseType => DatabaseType.Oracle;

    /// <summary>
    /// 重写以获取数据连接对象
    /// </summary>
    /// <param name="connectionString">数据连接字符串</param>
    /// <returns></returns>
    protected override IDbConnection GetDbConnection(string connectionString)
    {
        return new OracleConnection(connectionString);
    }
}
