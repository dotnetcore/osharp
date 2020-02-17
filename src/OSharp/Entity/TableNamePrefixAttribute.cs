// -----------------------------------------------------------------------
//  <copyright file="TableNamePrefixAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-17 14:36</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Entity
{
    /// <summary>
    /// 表名前缀特性，用于给实体类指定生成的表名前缀
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableNamePrefixAttribute : Attribute
    {
        /// <summary>
        /// 初始化一个<see cref="TableNamePrefixAttribute"/>类型的新实例
        /// </summary>
        public TableNamePrefixAttribute(string prefix)
        {
            Prefix = prefix;
        }

        /// <summary>
        /// 获取 表名前缀
        /// </summary>
        public string Prefix { get; }
    }
}