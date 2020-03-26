// -----------------------------------------------------------------------
//  <copyright file="SequentialGuidType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-26 9:52</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Data
{
    /// <summary>
    /// 有序Guid类型
    /// </summary>
    public enum SequentialGuidType
    {
        /// <summary>
        /// 将GUID当作字符串来处理的前排序，MySql，Sqlite，PostgreSQL中适用
        /// </summary>
        SequentialAsString,

        /// <summary>
        /// 将GUID作为二进制处理的排序，Oracle，PostgreSQL中适用
        /// </summary>
        SequentialAsBinary,

        /// <summary>
        /// 按GUID的最后一段来排序，SqlServer中适用
        /// </summary>
        SequentialAtEnd
    }
}