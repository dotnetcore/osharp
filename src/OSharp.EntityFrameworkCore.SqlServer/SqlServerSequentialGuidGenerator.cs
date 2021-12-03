﻿// -----------------------------------------------------------------------
//  <copyright file="SqlServerSequentialGuidGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-14 21:06</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Data;
using OSharp.Entity.KeyGenerate;


namespace OSharp.Entity.SqlServer
{
    /// <summary>
    /// SqlServer顺序
    /// </summary>
    public class SqlServerSequentialGuidGenerator : ISequentialGuidGenerator
    {
        /// <summary>
        /// 获取一个<see cref="Guid"/>类型的主键数据
        /// </summary>
        /// <returns></returns>
        public Guid Create()
        {
            return SequentialGuid.Create(SequentialGuidType.SequentialAtEnd);
        }

        /// <summary>
        /// 获取 顺序Guid数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; } = DatabaseType.SqlServer;
    }
}