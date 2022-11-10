// -----------------------------------------------------------------------
//  <copyright file="OracleSequentialGuidGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-11 1:28</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Entity.Oracle;

/// <summary>
/// Oracle数据库顺序Guid生成器
/// </summary>
public class OracleSequentialGuidGenerator : ISequentialGuidGenerator
{
    /// <summary>
    /// 获取一个<see cref="Guid"/>类型的主键数据
    /// </summary>
    /// <returns></returns>
    public Guid Create()
    {
        return SequentialGuid.Create(SequentialGuidType.SequentialAsBinary);
    }

    /// <summary>
    /// 获取 顺序Guid数据库类型
    /// </summary>
    public DatabaseType DatabaseType { get; } = DatabaseType.Oracle;
}
